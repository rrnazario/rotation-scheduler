using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Users;

public interface IUser : IAggregation
{
    string Name { get; }
    string Email { get; }
    string ExternalId { get; set; }
    ICalendar Calendar { get; }

    void FillCalendar(ICalendar calendar);

    CalendarAvailability GetAvailability(Duration date);
}
