using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;

namespace Rotation.API.Features.Activities;

public class Activity : IActivity
{

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Duration Duration { get; private set; }

    public Activity(string name, string description, Duration duration)
    {
        Name = name;
        Description = description;
        Duration = duration;
    }
}
