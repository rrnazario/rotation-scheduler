using Rotation.Domain.Activities;

namespace Rotation.Infra.Activities;

internal class ActivityRepository
    : IActivityRepository
{
    private static readonly List<IActivity> entities = new();

    public Task<Guid> AddAsync(IActivity entity, CancellationToken cancellationToken = default)
    {
        entities.Add(entity);


        return Task.FromResult(entity.Id);
    }

    public Task<IActivity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.Find(a => a.Id == id));
    }
}
