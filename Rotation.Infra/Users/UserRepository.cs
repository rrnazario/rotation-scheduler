using Rotation.Domain.Users;

namespace Rotation.Infra.Users;

internal class UserRepository
    : IUserRepository
{
    private static readonly List<IUser> entities = new();
    public Task<Guid> AddAsync(IUser entity, CancellationToken cancellationToken = default)
    {
        entities.Add(entity);


        return Task.FromResult(entity.Id);
    }

    public Task<IUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(entities.Find(a => a.Id == id));
    }
}
