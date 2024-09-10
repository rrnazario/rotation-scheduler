using Rotation.Application;
using Rotation.Application.Features.Calendar;
using Rotation.Domain.Calendars;
using Rotation.Domain.Users;

namespace Rotation.Infra.Users;

internal class UserRepository
    : IUserRepository
{
    private static readonly List<IUser> entities = TestData.Users;
    public Task<int> AddAsync(IUser entity, CancellationToken cancellationToken = default)
    {
        entities.Add(entity);

        return Task.FromResult(entity.Id);
    }

    public Task<IEnumerable<IUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            var calendar = new Calendar(Random.Shared.Next(), entity.Id);

            var begin = DateTime.UtcNow.AddDays(entities.IndexOf(entity) - 1);
            var days = new List<CalendarDay>()
            {
                new CalendarDay(begin, true),
                new CalendarDay(begin.AddDays(1), true),
                new CalendarDay(begin.AddDays(2), true),
                new CalendarDay(begin.AddDays(3), false),
                new CalendarDay(begin.AddDays(4), false),
                new CalendarDay(begin.AddDays(5), false),
            };

            calendar.FillDays(days);

            entity.FillCalendar(calendar);
        }

        return Task.FromResult(entities.AsEnumerable());
    }

    public Task<IUser[]?> GetByEmailsAsync(string[] userEmails, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.Where(a => userEmails.Contains(a.Email)).ToArray());
    }

    public Task<IUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.Find(a => a.Id == id));
    }
}
