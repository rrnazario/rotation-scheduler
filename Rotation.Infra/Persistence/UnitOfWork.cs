using Rotation.Domain.SeedWork;

namespace Rotation.Infra.Persistence;

internal class UnitOfWork
    : IUnitOfWork
{
    private readonly DatabaseContext _dbContext;

    public UnitOfWork(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);        
    }
}
