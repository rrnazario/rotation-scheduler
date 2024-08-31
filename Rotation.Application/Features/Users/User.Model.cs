using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Users;

public class User(string name, string login) 
    : IUser
{

    public string Name { get; private set; } = name;
    public string Login { get; private set; } = login;
    public ICalendar Calendar { get; private set; }

    public void FillCalendar(DateTime? begin = null, Duration? duration = null)
    {
        Console.WriteLine("Calendar filled");
    }
}
