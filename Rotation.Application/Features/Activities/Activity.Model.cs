using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Activities;

public class Activity(string name, string description, Duration duration) 
    : IActivity
{

    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public Duration Duration { get; private set; } = duration;
    public IEnumerable<IUser> Users { get; private set; } = [];

    public bool TryAddUser(IUser user)
    {
        if (Users.Any(u => u.Name == user.Name))
        {
            return false;
        }

        Users = Users.Concat([user]);

        return true;
    }
}
