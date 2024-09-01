using static Rotation.Infra.Services.Personio.PersonioServiceModels;

namespace Rotation.Infra.Services.Personio;

public interface IPersonioService
{
    Task<PersonioEmployeeResponse> GetEmployeeByEmail(string email, CancellationToken cancellationToken);
}
