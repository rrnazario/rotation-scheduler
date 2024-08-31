namespace Rotation.Domain.SeedWork;

public interface IEntity;
public interface IAggregation : IEntity;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<T>
    where T : IAggregation
{
    Task<Guid> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
