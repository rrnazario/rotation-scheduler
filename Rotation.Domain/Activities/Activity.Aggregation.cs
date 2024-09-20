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

public record ActivityResume(
    string MainName,
    string MainEmail,
    string? ReplacerName,
    string? ReplacerEmail,
    DateTime CurrentBegin,
    DateTime CurrentEnd,
    string ActivityName,
    IUser[] UnavailableUsers)
{
    public override string ToString()
        => $"Activity '{ActivityName}'\n" +
           $"From: {CurrentBegin:dd/MM/yyyy HH:mm}\n" +
           $"To: {CurrentEnd:dd/MM/yyyy HH:mm}\n" +
           $"In Charge: {MainName} ({MainEmail})\n" +
           $"Replacer: {ReplacerName} ({ReplacerEmail})";
}