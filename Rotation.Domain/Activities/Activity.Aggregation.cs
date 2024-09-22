using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;

namespace Rotation.Domain.Activities;

public interface IActivity : IAggregation
{
    public string Name { get; }
    public string Description { get; }
    public Duration Duration { get; }
    public ICollection<IUser> Users { get; }
    bool TryAddUser(IUser user);
    void Rotate();
    IActivityResume GetActivityResume();
}

public interface IActivityResume;