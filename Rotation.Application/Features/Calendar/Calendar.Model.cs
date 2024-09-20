using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Application.Features.Calendar;

public class Calendar
    : ICalendar
{
    public int Id { get; }
    public int UserId { get; }

    public Calendar(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }

    public IEnumerable<CalendarDay> Days { get; private set; } = [];

    public void FillDays(IEnumerable<CalendarDay> days) => Days = days;

    public CalendarAvailability GetAvailability(Duration duration)
    {
        var result = new List<CalendarDay>();
        var durationDays = duration.GetCurrentInterval(); //01/09 a 14/09

        foreach (var durationDay in durationDays)
        {
            var day = Days.FirstOrDefault(d => d.Date.Date == durationDay.Date);

            result.Add(day ?? new CalendarDay(durationDay.Date, false));
        }

        return new CalendarAvailability(result);
    }
}