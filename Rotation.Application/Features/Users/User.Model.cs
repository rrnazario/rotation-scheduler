using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Infra.Features.Users;

public class User : IUser
{
    public User(string name, string email)
    {
        Name = name;
        Email = email;
        Id = Guid.NewGuid();
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public ICalendar Calendar { get; private set; }

    public Guid Id {  get; private set; }

    public void FillCalendar(ICalendar calendar) => Calendar = calendar;

    public bool IsAvailable(Duration duration) => Calendar is null || Calendar.IsAvailable(duration);
}
