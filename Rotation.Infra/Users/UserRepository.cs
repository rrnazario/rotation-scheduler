using Microsoft.EntityFrameworkCore;
using Rotation.Application.Features.Users;
using Rotation.Domain.Users;
using Rotation.Infra.Persistence;

namespace Rotation.Infra.Users;

internal class UserRepository
    : IUserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IUser> AddAsync(IUser entity, CancellationToken cancellationToken = default)
    {
        var user = await _databaseContext.Set<User>().AddAsync(entity as User, cancellationToken);

        return user.Entity;
    }

    public async Task<IEnumerable<IUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Set<User>().ToListAsync(cancellationToken);
    }

    public async Task<IUser[]?> GetByEmailsAsync(string[] userEmails, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Set<User>().Where(w => userEmails.Contains(w.Email))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Set<User>().FindAsync(id, cancellationToken);
    }
}