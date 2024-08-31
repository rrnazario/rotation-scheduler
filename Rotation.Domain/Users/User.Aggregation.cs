using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Users;

public interface IUser : IAggregation
{
    string Name { get; }
    string? Login { get; }
    ICalendar Calendar { get; }

    void FillCalendar(ICalendar calendar);

    bool IsAvailable(Duration date);
}
