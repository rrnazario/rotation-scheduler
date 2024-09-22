using FluentValidation;
using MediatR;
using Rotation.Application.Features.Activities;
using Rotation.Domain.Activities;
using Rotation.Domain.Exceptions;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Contracts;
using Rotation.Infra.Services.Personio;
using Rotation.Infra.Services.Personio.Models;
using static Rotation.API.Activities.Features.GetNextUserOnRotation;

namespace Rotation.API.Activities.Features;

public static class GetNextUserOnRotation
{
    internal record GetActivityQuery(int ActivityId)
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
        private readonly IPersonioClient _personioClient;

        public Handler(
            IMediator metiator,
            IActivityRepository repository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPersonioClient personioClient)
        {
            _mediator = metiator;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _personioClient = personioClient;
        }

        public async Task<ActivityResume> Handle(GetActivityQuery request, CancellationToken cancellationToken)
        {
            var activity = (Activity)await _repository.GetByIdAsync(request.ActivityId, cancellationToken);
            if (activity is null)
            {
                throw new EntityNotFoundException(nameof(activity));
            }

            var users = await _userRepository.GetAllAsync(cancellationToken);
            foreach (var user in users)
            {
                activity.TryAddUser(user);
            }

            var resume = await GetActivityResumeAsync(activity, cancellationToken);

            return resume;
        }

        private async Task<ActivityResume> GetActivityResumeAsync(Activity activity,
            CancellationToken cancellationToken)
        {
            var begin = activity.Duration.CurrentBegin.Date;
            var end = activity.Duration.CurrentEnd().Date;

            if (activity.Resume is not null && 
                activity.Resume.CurrentBegin.Date == begin &&
                activity.Resume.CurrentEnd.Date == end)
            {
                return activity.GetActivityResume();
            }

            var request =
                new PersonioTimeOffModels.GetTimeOffAsyncRequest(begin, end,
                    activity.Users.Select(s => s.ExternalId).ToArray());
            var timeoffs = (await _personioClient.GetTimeOffAsync(request, cancellationToken))
                .GroupBy(d => d.EmployeeEmail)
                .ToDictionary(d => d.Key, v => v.AsEnumerable().OrderBy(o => o.StartDate).ToArray());

            foreach (var email in timeoffs.Keys)
            {
                var user = activity.Users.FirstOrDefault(f => f.Email == email);
                if (user is null) continue;

                var calendar = new Calendar();

                var daysOff = timeoffs[email];

                calendar.FillDays(
                    activity.Duration.GetCurrentInterval().Select(s =>
                    {
                        var dayOff = daysOff.FirstOrDefault(f => f.StartDate.Date == s.Date);

                        return new CalendarDay(s.Date, Available: dayOff is null);
                    }).ToArray());

                user.FillCalendar(calendar);
            }

            var resume = activity.GetActivityResume();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return resume;
        }
    }
}

public class GetActivityModule : IEndpointModule
{
    public void Map(IEndpointRouteBuilder app)
        => app
            .MapGet(
                ActivityConstants.Route + "/{activityId}",
                async (ISender sender, int activityId)
                    => await sender.Send(new GetActivityQuery(activityId)))
            .Produces<ActivityResume>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
}