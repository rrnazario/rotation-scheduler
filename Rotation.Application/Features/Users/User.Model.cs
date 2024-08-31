using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Users;

public class User : IUser
{
    public User(string name, string login)
    {
        Name = name;
        Login = login;
        Id = Guid.NewGuid();
    }

    public string Name { get; private set; }
    public string Login { get; private set; }
    public ICalendar Calendar { get; private set; }

    public Guid Id {  get; private set; }

    public void FillCalendar(DateTime? begin = null, Duration? duration = null)
    {
        Console.WriteLine("Calendar filled");
    }
}
