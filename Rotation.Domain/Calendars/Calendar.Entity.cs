using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Calendars;

public interface ICalendar
    : IAggregation
{
    public int Id { get; }
    public int UserId { get; }
    public CalendarDay[] Days { get; }

    void FillDays(DateTime? begin = null);
}

public record CalendarDay(DateTime date, bool available);
