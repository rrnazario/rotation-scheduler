using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    internal record GetActivityQuery(Guid ActivityId)
        : IRequest<ActivityResume>;
    class Validator
    : AbstractValidator<GetActivityQuery>
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

    record Handler : IRequestHandler<GetActivityQuery, ActivityResume>
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

        public async Task<ActivityResume> Handle(GetActivityQuery request, CancellationToken cancellationToken)
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

            return activity.GetActivityResume();
        }
    }
}

public class GetActivityModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapGet(
            ActivityConstants.Route + "/{activityId}",
            async (ISender sender, Guid activityId) 
               => await sender.Send(new GetActivityQuery(activityId)))
           .IncludeInOpenApi()
           .Produces<ActivityResume>()
           .ProducesProblem(StatusCodes.Status400BadRequest);
}
