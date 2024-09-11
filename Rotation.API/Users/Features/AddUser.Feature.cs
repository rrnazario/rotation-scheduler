using Carter;
using Carter.OpenApi;
using FluentValidation;
using MediatR;
using Rotation.Application.Features.Users;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.API.Users.Features;

public static class AddUser
{
    internal record AddUserCommand(
        string Name,
        string Email)
        : IRequest<AddUserResponse>;

    internal record AddUserResponse(int Id);

    class Validator
    : AbstractValidator<AddUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;
        public Validator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            RuleFor(_ => _.Name)
                .NotEmpty()
                .WithMessage("Name must have a value");

            RuleFor(_ => _.Email)
                .NotEmpty()
                .WithMessage("Email must have a value");
                //.Must(e => e); // add email validation

        }
    }

    record Handler : IRequestHandler<AddUserCommand, AddUserResponse>
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

        public async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new User(request.Name, request.Email);

            var newId = await _repository.AddAsync(entity, cancellationToken);
            var response = new AddUserResponse(newId);

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
            .MapPost<AddUser.AddUserCommand>(
            UserConstants.Route,
            async (ISender sender, AddUser.AddUserCommand command) =>
            {
                var response = await sender.Send(command);

                return Results.Created(UserConstants.Route, response);
            })
           .IncludeInOpenApi()
           .Produces<AddUser.AddUserResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
}
