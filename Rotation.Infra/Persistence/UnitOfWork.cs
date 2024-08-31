using Rotation.Domain.SeedWork;

namespace Rotation.Infra.Persistence;

internal class UnitOfWork
    : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
