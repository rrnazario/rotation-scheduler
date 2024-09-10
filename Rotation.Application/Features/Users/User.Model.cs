﻿using Rotation.Domain.Calendars;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Users;

public class User : IUser
{
    public User(string name, string email)
    {
        Name = name;
        Email = email;
        Id = 1;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public ICalendar Calendar { get; private set; }
    public int Id {  get; private set; }

    public void FillCalendar(ICalendar calendar) => Calendar = calendar;

    public CalendarAvailability GetAvailability(Duration duration) => Calendar.GetAvailability(duration);
}
