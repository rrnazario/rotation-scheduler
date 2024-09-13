using static Rotation.Infra.Services.Personio.Models.PersonioEmployeeModels;

namespace Rotation.Infra.Services.Personio;

public interface IPersonioClient
{
    Task<PersonioEmployeeResponse> GetEmployeeByEmail(string email, CancellationToken cancellationToken);
}
