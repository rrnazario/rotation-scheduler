using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Users;

public interface IUser : IAggregation
{
    string Name { get; }
    string Email { get; }
    string ExternalId { get; set; }
    Calendar Calendar { get; }

    void FillCalendar(Calendar calendar);

    CalendarAvailability GetAvailability(Duration date);
}
