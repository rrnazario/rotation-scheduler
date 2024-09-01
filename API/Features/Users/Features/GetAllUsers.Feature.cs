using Carter;
using Carter.OpenApi;
using MediatR;
using Rotation.Infra.Features.Users;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using static Rotation.API.Features.Users.Features.GetAllUsers;

namespace Rotation.API.Features.Users.Features;

public static class GetAllUsers
{
    internal record GetAllUsersQuery 
        : IRequest<GetUsersResponse>;
    internal record GetUsersResponse(IEnumerable<IUser> Users);

    record Handler : IRequestHandler<GetAllUsersQuery, GetUsersResponse>
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

        public async Task<GetUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return new(await _repository.GetAllAsync(cancellationToken));
        }
    }
}

public class GetAllUsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapGet(
            UserConstants.Route,
            async (ISender sender) =>
            {
                return await sender.Send(new GetAllUsersQuery());
            })
           .IncludeInOpenApi()
           .Produces<GetUsersResponse>(StatusCodes.Status200OK);
}
