using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Rotation.Application.Features.Users;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.API.Features.Users;

public static class AddUser
{
    internal record Command(
        string Name, 
        string Login) 
        : IRequest<Response>;

    internal record Response(Guid Id);

    class Validator
    : AbstractValidator<Command>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Name must have a value");

            RuleFor(_ => _.Login)
                .NotEmpty()
                .WithMessage("Login must have a value");

        }
    }

    record Handler : IRequestHandler<Command, Response>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(
            IMediator metiator,
            IUserRepository repository,
            IUnitOfWork unitOfWork)
        {
            _mediator = metiator;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = new User(request.Name, request.Login);

            var newId = await _repository.AddAsync(entity, cancellationToken);
            var response = new Response(newId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //await _mediator.Publish(response);

            return response;
        }
    }
}

public class AddUserModule : ICarterModule
{   
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapPost<AddUser.Command>(
            UserConstants.Route,
            async (ISender sender, AddUser.Command command) =>
            {
                var response = await sender.Send(command);

                return Results.Created(UserConstants.Route, response);
            })
           .IncludeInOpenApi()
           .Produces<AddUser.Response>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
}
