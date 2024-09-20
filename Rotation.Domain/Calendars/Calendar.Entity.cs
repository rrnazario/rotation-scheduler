using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Calendars;

public interface ICalendar
{
    public IEnumerable<CalendarDay> Days { get; }

    void FillDays(IEnumerable<CalendarDay> days);

    CalendarAvailability GetAvailability(Duration duration);
}

public record CalendarDay(DateTime Date, bool Available);

public record CalendarAvailability(List<CalendarDay> Availability)
{
    public int AvailabilityPercentage => Availability.Count(v => v.Available) * 100 / Availability.Count;
}
