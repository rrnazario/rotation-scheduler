using Rotation.Application.Features.Activities;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;

namespace Rotation.Infra.Activities;

internal class ActivityRepository
    : IActivityRepository
{
    private static readonly List<IActivity> entities = new()
    {
        new Activity("On Duty", "On duty call", new Duration(4, DurationType.Days, DateTime.UtcNow.Date))
    };

    public Task<Guid> AddAsync(IActivity entity, CancellationToken cancellationToken = default)
    {
        entities.Add(entity);


        return Task.FromResult(entity.Id);
    }

    public Task<IEnumerable<IActivity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.AsEnumerable());
    }

    public Task<IActivity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.Find(a => a.Id == id));
    }
}
