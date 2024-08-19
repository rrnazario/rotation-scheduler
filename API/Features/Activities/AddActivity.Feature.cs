using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;

namespace Rotation.API.Features.Activities;

public static class AddActivity
{
    internal record Command(
        string Name, 
        string Description, 
        Duration Duration) 
        : IRequest<Response>;

    internal record Response(Guid Id);

    class Validator
    : AbstractValidator<Command>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            //RuleFor(_ => _.Name)
            //    .NotEmpty()
            //    .WithMessage("Name must have a value");

            //RuleFor(_ => _.Description)
            //    .NotEmpty()
            //    .WithMessage("Description must have a value");

            RuleFor(_ => _.Duration)
                .Must(d => d.IsValid())
                .WithMessage("Duration must have a valid value");
        }
    }

    record Handler : IRequestHandler<Command, Response>
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

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = new Activity(request.Name, request.Description, request.Duration);

            var newId = await _repository.AddAsync(activity, cancellationToken);
            var response = new Response(newId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //await _mediator.Publish(response);

            return response;
        }
    }
}

public class AddActivityModule : ICarterModule
{
    private const string _route = "/activity";
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapPost<AddActivity.Command>(
            _route,
            async (ISender sender, AddActivity.Command command) =>
            {
                var response = await sender.Send(command);

                return Results.Created(_route, response);
            })
           .IncludeInOpenApi()
           .Produces<AddActivity.Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
}
