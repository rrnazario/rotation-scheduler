using Rotation.Application.Features.Users;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rotation.Application.Features.Activities;

public class Activity : IActivity
{
    public Activity()
    {
    }

    public Activity(string name, string description, Duration duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Duration Duration { get; private set; }
    public ActivityResume? Resume { get; private set; }
    public ICollection<User> Users { get; set; }

    [NotMapped] ICollection<IUser> IActivity.Users => Users as ICollection<IUser>;

    public int Id { get; private set; }


    public bool TryAddUser(IUser user)
    {
        Users ??= new List<User>();

        if (Users.Any(u => u.Email == user.Email))
        {
            return false;
        }

        Users.Add((User)user);

        return true;
    }

    public ActivityResume GetActivityResume()
    {
        if (Resume is not null)
            return Resume;

        IUser? main = default, replacer = default;
        var unavailableUsers = new List<IUser>();
        foreach (var user in Users)
        {
            var availability = user.GetAvailability(Duration);
            var availabilityPercentage = availability.AvailabilityPercentage;

            if (availabilityPercentage > 70)
            {
                if (main is null)
                {
                    main = user;
                    continue;
                }

                if (replacer is null)
                {
                    replacer = user;
                    continue;
                }

                break;
            }

            unavailableUsers.Add(user);
        }

        Resume = new ActivityResume(main.Name, main.Email, replacer.Name, replacer.Email, Duration.CurrentBegin,
            Duration.CurrentEnd(), Name,
            unavailableUsers.ToArray());

        return Resume;
    }

    public void Rotate()
    {
        var resume = GetActivityResume();

        MoveMainUserToEnd(resume.MainEmail);
        MoveNotAvailableUsersTopList(resume.UnavailableUsers.ToList());
        Duration.SetNextBegin();
    }

    private void MoveMainUserToEnd(string email)
    {
        var currentUsers = Users.ToList();
        var user = currentUsers.Find(e => e.Email == email);
        currentUsers.Remove(user!);

        currentUsers.Append(user!);
    }

    private void MoveNotAvailableUsersTopList(List<IUser> users)
    {
        if (!users.Any()) return;

        var currentUsers = Users.ToList();
        users.Reverse();

        foreach (var user in users)
        {
            currentUsers.Remove((User)user!);
            currentUsers.Insert(0, (User)user!);
        }

        Users = currentUsers;
    }
}