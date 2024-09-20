using Rotation.Infra.Services.Personio.Models;
using static Rotation.Infra.Services.Personio.Models.PersonioEmployeeModels;

namespace Rotation.Infra.Services.Personio;

public interface IPersonioClient
{
    Task<PersonioEmployeeResponse> GetEmployeeByEmail(string email, CancellationToken cancellationToken);
    Task<PersonioTimeOffModels.PersonioTimeOffResponse[]> GetTimeOffAsync(PersonioTimeOffModels.GetTimeOffAsyncRequest request,
        CancellationToken cancellationToken);
}
