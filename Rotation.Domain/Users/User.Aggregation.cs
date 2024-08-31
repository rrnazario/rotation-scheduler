using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Users;

public interface IUser : IAggregation
{
    string Name { get; }
    string? Login { get; }
    ICalendar Calendar { get; }

    void FillCalendar(DateTime? begin = null, Duration? duration = null);
}
