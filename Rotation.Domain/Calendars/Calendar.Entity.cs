using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Calendars;

public interface ICalendar
    : IAggregation
{
    public int UserId { get; }

    public IEnumerable<CalendarDay> Days { get; }

    void FillDays(IEnumerable<CalendarDay> days);

    CalendarAvailability GetAvailability(Duration duration);
}

public record CalendarDay(DateTime Date, bool Available);

public record CalendarAvailability(Dictionary<DateTime, bool> Availability)
{
    public int AvailabilityPercentage => Availability.Values.Count(v => v) * 100 / Availability.Count;
}
