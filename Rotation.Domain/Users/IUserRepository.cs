using Rotation.Domain.SeedWork;

namespace Rotation.Domain.Users;

public interface IUserRepository
    : IRepository<IUser>
{
    Task<IUser[]?> GetByEmailsAsync(string[] userEmails, CancellationToken cancellationToken = default);
}
