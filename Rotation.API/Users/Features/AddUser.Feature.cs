using FluentValidation;
using MediatR;
using Rotation.Application.Features.Users;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Contracts;
using Rotation.Infra.Services.Personio;

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
        private readonly IPersonioClient _personioClient;

        public Handler(
            IMediator metiator,
            IUserRepository repository,
            IUnitOfWork unitOfWork,
            IPersonioClient client)
        {
            _mediator = metiator;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _personioClient = client;
        }

        public async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            bool infoChanged = false;
            var entityUser = await _repository.GetByEmailsAsync([request.Email], cancellationToken);

            if (entityUser is not { Length: > 0 } || entityUser[0] is not User user)
            {
                user = new User(request.Name, request.Email);

                user = (User)await _repository.AddAsync(user, cancellationToken);

                infoChanged = true;
            }

            infoChanged |= await TryUpdatePersonioInfoAsync(user, cancellationToken);

            if (infoChanged)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return new AddUserResponse(user.Id);
        }

        private async Task<bool> TryUpdatePersonioInfoAsync(IUser entity, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(entity.ExternalId)) 
                return false;

            var personioUser = await _personioClient.GetEmployeeByEmail(entity.Email, cancellationToken);

            entity.ExternalId = personioUser.id;

            return true;
        }
    }
}

public class AddUserModule : IEndpointModule
{
    public void Map(IEndpointRouteBuilder app)
        => app.MapPost(
                UserConstants.Route,
                async (ISender sender, AddUser.AddUserCommand command) =>
                {
                    var response = await sender.Send(command);

                    return Results.Created(UserConstants.Route, response);
                })
            .Produces<AddUser.AddUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
}