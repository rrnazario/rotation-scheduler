using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;
using Rotation.Application.Features.Activities;

namespace Rotation.Application.Features.Users;

public class User : IUser
{
    private User() { }

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
    
    public ICollection<Activity> Activities { get; set; }

    [NotMapped] public ICalendar Calendar { get; private set; } = new Calendar.Calendar();

    public void FillCalendar(ICalendar calendar) => Calendar = calendar;

    public CalendarAvailability GetAvailability(Duration duration) => Calendar.GetAvailability(duration);
}
