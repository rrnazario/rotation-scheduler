using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Calendars;

public interface ICalendar
    : IAggregation
{
    public Guid UserId { get; }

    public IEnumerable<CalendarDay> Days { get; }

    void FillDays(IEnumerable<CalendarDay> days);

    bool IsAvailable(Duration duration);
}

public record CalendarDay(DateTime Date, bool Available);
