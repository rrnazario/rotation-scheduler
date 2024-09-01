using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Infra.Features.Calendar;

public class Calendar
    : ICalendar
{
    public Guid Id { get; }
    public Guid UserId { get; }

    public Calendar(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public IEnumerable<CalendarDay> Days { get; private set; } = [];

    public void FillDays(IEnumerable<CalendarDay> days) => Days = days;

    public bool IsAvailable(Duration duration)
    {
        var durationDays = duration.GetCurrentInterval(); //01/09 a 14/09

        foreach (var durationDay in durationDays)
        {
            var day = Days.FirstOrDefault(d => d.Date.Date == durationDay.Date);

            if (day is not null && !day.Available)
                return false;
        }

        return true;
    }
}
