namespace Rotation.Domain.SeedWork;

public interface IEntity;
public interface IAggregation : IEntity
{
    int Id { get; }
};

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<T>
    where T : IAggregation
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
}
