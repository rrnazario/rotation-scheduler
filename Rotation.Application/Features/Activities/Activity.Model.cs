using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Application.Features.Activities;

public class Activity : IActivity
{
    public Activity(string name, string description, Duration duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Id = Guid.NewGuid();
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Duration Duration { get; private set; }
    public IEnumerable<IUser> Users { get; private set; } = [];

    public Guid Id {  get; private set; }

    public bool TryAddUser(IUser user)
    {
        if (Users.Any(u => u.Name == user.Name))
        {
            return false;
        }

        Users = Users.Concat([user]);

        return true;
    }

    public (IUser Main, IUser? Replacer) GetNextUsersOnRotation()
    {
        IUser? main = default, replacer = default;
        foreach (var user in Users)
        {
            if (user.IsAvailable(Duration) && main is null)
                main = user;
        }
        
        throw new NotImplementedException();
    }
}
