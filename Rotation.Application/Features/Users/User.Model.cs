using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Users;

public class User : IUser
{
    public User(string name, string email, string externalId = "")
    {
        Name = name;
        Email = email;
        ExternalId = externalId;
    }

    public int Id {  get; private set; }
    public string ExternalId {  get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public ICalendar Calendar { get; private set; }

    public void FillCalendar(ICalendar calendar) => Calendar = calendar;

    public CalendarAvailability GetAvailability(Duration duration) => Calendar.GetAvailability(duration);
}
