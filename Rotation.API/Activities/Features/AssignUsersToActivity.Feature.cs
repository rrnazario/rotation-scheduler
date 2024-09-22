using FluentValidation;
using MediatR;
using Rotation.Domain.Activities;
using Rotation.Domain.Exceptions;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using static Rotation.API.Activities.ActivityExceptions;
using static Rotation.API.Activities.Features.AssignUsersToActivity;

namespace Rotation.API.Activities.Features;

public static class AssignUsersToActivity
{
    internal record AssignUsersToActivityCommand(
        string[] UserEmails)
        : IRequest
    {
        [NotMapped] public int ActivityId { get; set; }
    }

    class Validator
    : AbstractValidator<AssignUsersToActivityCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.UserEmails)
                .NotEmpty()
                .WithMessage("UserEmails must have a value");
        }
    }

    record Handler : IRequestHandler<AssignUsersToActivityCommand>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IActivityRepository repository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            ILogger<Handler> logger)
        {
            _activityRepository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(AssignUsersToActivityCommand command, CancellationToken cancellationToken)
        {
            var activity = await _activityRepository.GetByIdAsync(command.ActivityId, cancellationToken);
            if (activity is null)
            {
                throw new EntityNotFoundException(nameof(activity));
            }

            var users = await _userRepository.GetByEmailsAsync(command.UserEmails, cancellationToken);
            if (users is null || !users.Any())
            {
                throw new EntityNotFoundException(nameof(users));
            }

            foreach (var user in users)
            {
                var userAdded = activity.TryAddUser(user);

                if (!userAdded)
                {
                    _logger.LogWarning("User {email} already added to activity {activity}.", user.Email, activity.Name);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //await _mediator.Publish(response);
        }
    }
}

public class AssignUserToActivityModule
    : IEndpointModule
{
    public void Map(IEndpointRouteBuilder routeBuilder) 
        => routeBuilder
            .MapPut(
            ActivityConstants.Route + "/{activityId}/user",
            async (ISender sender, int activityId, AssignUsersToActivityCommand command) =>
            {
                try
                {
                    command.ActivityId = activityId;

                    await sender.Send(command);
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
           .Produces(StatusCodes.Status204NoContent)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
           .ProducesProblem(StatusCodes.Status400BadRequest);
}
