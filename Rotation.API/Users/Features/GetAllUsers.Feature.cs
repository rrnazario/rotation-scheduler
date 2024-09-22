using MediatR;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Contracts;
using static Rotation.API.Users.Features.GetAllUsers;

namespace Rotation.API.Users.Features;

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

public class GetAllUsersModule : IEndpointModule
{
    public void Map(IEndpointRouteBuilder app)
    => app
            .MapGet(
            UserConstants.Route,
            async (ISender sender) =>
            {
                return await sender.Send(new GetAllUsersQuery());
            })
           .Produces<GetUsersResponse>(StatusCodes.Status200OK);
}
