using Rotation.Domain.Activities;

namespace Rotation.Infra.Activities;

internal class ActivityRepository
    : IActivityRepository
{
    public Task<Guid> AddAsync(IActivity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Guid.Empty);
    }
}
