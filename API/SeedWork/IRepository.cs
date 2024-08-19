using Rotation.Domain.SeedWork;

namespace Rotation.API.SeedWork;

public interface IRepository<T>
    where T : IEntity, IAggregation
{
    Task<Guid> AddAsync(T entity, CancellationToken cancellationToken = default);
}
