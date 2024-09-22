using FluentValidation;
using MediatR;
using Rotation.Application.Features.Activities;
using Rotation.Domain.Activities;
using Rotation.Domain.Exceptions;
using Rotation.Domain.SeedWork;
using Rotation.Infra.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rotation.API.Activities.Features;

public static class UpdateActivity
{
    internal record UpdateActivityCommand(
        string? Name = null,
        string? Description = null,
        Duration? Duration = null)
        : IRequest
    {
        [NotMapped] public int ActivityId { get; set; }
    }

    class Validator
    : AbstractValidator<UpdateActivityCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Name must have a value");

            RuleFor(_ => _.Description)
                .NotEmpty()
                .WithMessage("Description must have a value");

            RuleFor(_ => _.Duration)
                .Must(d => d.IsValid())
                .WithMessage("Duration must have a valid value");
        }
    }

    record Handler : IRequestHandler<UpdateActivityCommand>
    {
        private readonly IActivityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IActivityRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = (Activity)await _repository.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity is null)
            {
                throw new EntityNotFoundException(nameof(activity));
            }

            var updated = activity.TryUpdate(request.Name, request.Description, request.Duration);

            if (updated)
                await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

public class UpdateActivityModule 
    : IEndpointModule
{
    public void Map(IEndpointRouteBuilder routeBuilder)
            => routeBuilder
            .MapPut(
            ActivityConstants.Route + "/{activityId}",
            async (ISender sender, int activityId, UpdateActivity.UpdateActivityCommand command) =>
            {
                command.ActivityId = activityId;

                await sender.Send(command);

                return Results.NoContent();
            })
           .Produces(StatusCodes.Status204NoContent)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

}
