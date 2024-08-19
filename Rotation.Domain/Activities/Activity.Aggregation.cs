using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Activities;

public interface IActivity : IAggregation
{
    public string Name { get; }
    public string Description { get; }
    public Duration Duration { get; }
}
