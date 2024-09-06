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

    public CalendarAvailability GetAvailability(Duration duration)
    {
        var result = new Dictionary<DateTime, bool>();
        var durationDays = duration.GetCurrentInterval(); //01/09 a 14/09

        foreach (var durationDay in durationDays)
        {
            var day = Days.FirstOrDefault(d => d.Date.Date == durationDay.Date);

            result.Add(durationDay.Date, day is { Available: true });
        }

        return new CalendarAvailability(result);
    }
}