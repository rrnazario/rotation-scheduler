namespace Rotation.Domain.SeedWork;

public interface IAggregation;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<T>
    where T : IAggregation
{
    Task<Guid> AddAsync(T entity, CancellationToken cancellationToken = default);
}
