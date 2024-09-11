using Carter;
using Carter.OpenApi;
using MediatR;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using static Rotation.API.Activities.Features.GetAllActivities;

namespace Rotation.API.Activities.Features;

public static class GetAllActivities
{
    internal record GetAllActivitiesQuery
        : IRequest<GetAllActivitiesResponse>;
    internal record GetAllActivitiesResponse(IEnumerable<IActivity> Activities);

    record Handler : IRequestHandler<GetAllActivitiesQuery, GetAllActivitiesResponse>
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

        public async Task<GetAllActivitiesResponse> Handle(GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            return new(await _repository.GetAllAsync(cancellationToken));
        }
    }
}

public class GetAllActivitiesModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    => app
            .MapGet(
            ActivityConstants.Route,
            async (ISender sender) =>
            {
                return await sender.Send(new GetAllActivitiesQuery());
            })
           .IncludeInOpenApi()
           .Produces<GetAllActivitiesResponse>(StatusCodes.Status200OK);
}
