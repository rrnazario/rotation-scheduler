using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Rotation.Domain.Activities;
using Rotation.Domain.Exceptions;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using static Rotation.API.Features.Activities.ActivityExceptions;

namespace Rotation.API.Features.Activities;

public static class AssignUserToActivity
{
    internal record Command(
        Guid UserId,
        Guid ActivityId)
        : IRequest;

    class Validator
    : AbstractValidator<Command>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.UserId)
                .NotEmpty()
                .WithMessage("Name must have a value");

            RuleFor(_ => _.ActivityId)
                .NotEmpty()
                .WithMessage("Description must have a value");
        }
    }

    record Handler : IRequestHandler<Command>
    {
        private readonly IMediator _mediator;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IMediator metiator,
            IActivityRepository repository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _mediator = metiator;
            _activityRepository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
            if (activity is null)
            {
                throw new EntityNotFoundException("Activity not found");
            }

            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
            if (user is null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var userAdded = activity.TryAddUser(user);

            if (!userAdded)
            {
                throw new UserAlreadyAddedException(command.UserId, command.ActivityId);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //await _mediator.Publish(response);
        }
    }
}

public class AssignUserToActivityModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapPut<AddActivity.Command>(
            $"{ActivityConstants.Route}/user",
            async (ISender sender, AddActivity.Command command) =>
            {
                try
                {
                    var response = await sender.Send(command);
                }
                catch (EntityNotFoundException e)
                {
                    return Results.UnprocessableEntity(e.Message);
                }
                catch (UserAlreadyAddedException e)
                {

                    return Results.BadRequest(e.Message);
                }

                return Results.NoContent();
            })
           .IncludeInOpenApi()
           .Produces<AddActivity.Response>(StatusCodes.Status204NoContent)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
           .ProducesProblem(StatusCodes.Status400BadRequest);
}
