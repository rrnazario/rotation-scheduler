using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Rotation.API.Features.Activities;
using Rotation.Domain.Activities;
using Rotation.Domain.Exceptions;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using System.Diagnostics;
using static Rotation.API.Features.Users.Features.GetNextUserOnRotation;

namespace Rotation.API.Features.Users.Features;

public static class GetNextUserOnRotation
{
    internal record GetNextUserOnRotationQuery(Guid ActivityId)
        : IRequest<GetNextUserOnRotationResponse>;
    internal record GetNextUserOnRotationResponse(IUser MainUser, IUser? Replacer);

    class Validator
    : AbstractValidator<GetNextUserOnRotationQuery>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.ActivityId)
                .NotEmpty()
                .WithMessage("ActivityID must have a value");
        }
    }

    record Handler : IRequestHandler<GetNextUserOnRotationQuery, GetNextUserOnRotationResponse>
    {
        private readonly IMediator _mediator;
        private readonly IActivityRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IMediator metiator,
            IActivityRepository repository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _mediator = metiator;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<GetNextUserOnRotationResponse> Handle(GetNextUserOnRotationQuery request, CancellationToken cancellationToken)
        {
            var activity = await _repository.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity is null)
            {
                throw new EntityNotFoundException(nameof(activity));
            }

            var users = await _userRepository.GetAllAsync(cancellationToken);
            foreach (var user in users)
            {
                activity.TryAddUser(user);
            }

            var result = activity.GetNextUsersOnRotation();
            return new(result.Main, result.Replacer);
        }
    }
}

public class GetNextUserOnRotationModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapGet(
            ActivityConstants.Route + "/rotation/{activityId}",
            async (ISender sender, Guid activityId) =>
            {
                return await sender.Send(new GetNextUserOnRotationQuery(activityId));
            })
           .IncludeInOpenApi()
           .Produces<GetNextUserOnRotationResponse>(StatusCodes.Status200OK);
}
