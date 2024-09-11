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
    ActivityResume GetActivityResume();
}

public record ActivityResume(IUser Main, IUser? Replacer, DateTime CurrentBegin, DateTime CurrentEnd, string Name, IUser[] UnavailableUsers)
{
    public override string ToString()
        => $"Activity '{Name}'\n" +
           $"In Charge: {Main.Name} ({Main.Email})\n" +
           $"Replacer: {Replacer?.Name} ({Replacer?.Email})\n" +
           $"From: {CurrentBegin:dd/MM/yyyy HH:mm}\n" +
           $"To: {CurrentEnd:dd/MM/yyyy HH:mm}";
}
