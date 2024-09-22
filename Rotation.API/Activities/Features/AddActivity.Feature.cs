using FluentValidation;
using MediatR;
using Rotation.Application.Features.Activities;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Infra.Contracts;

namespace Rotation.API.Activities.Features;

public static class AddActivity
{
    internal record AddActivityCommand(
        string Name,
        string Description,
        Duration Duration)
        : IRequest<AddActivityResponse>;

    internal record AddActivityResponse(int Id);

    class Validator
    : AbstractValidator<AddActivityCommand>
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

    record Handler : IRequestHandler<AddActivityCommand, AddActivityResponse>
    {
        private readonly IMediator _mediator;
        private readonly IActivityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IMediator metiator,
            IActivityRepository repository,
            IUnitOfWork unitOfWork)
        {
            _mediator = metiator;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddActivityResponse> Handle(AddActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = new Activity(request.Name, request.Description, request.Duration);

            var newActivity = await _repository.AddAsync(activity, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AddActivityResponse(newActivity.Id);
            //await _mediator.Publish(response);

            return response;
        }
    }
}

public class AddActivityModule 
    : IEndpointModule
{
    public void Map(IEndpointRouteBuilder routeBuilder)
            => routeBuilder
            .MapPost(
            ActivityConstants.Route,
            async (ISender sender, AddActivity.AddActivityCommand command) =>
            {
                var response = await sender.Send(command);

                return Results.Created(ActivityConstants.Route, response);
            })
           .Produces<AddActivity.AddActivityResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

}
