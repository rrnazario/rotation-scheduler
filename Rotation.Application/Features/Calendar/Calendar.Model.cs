using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Application.Features.Calendar;

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
        var day = Days.FirstOrDefault(d => d.Date.Date == duration.CurrentBegin.Date);

        return day is null || day.Available;
    }
}
