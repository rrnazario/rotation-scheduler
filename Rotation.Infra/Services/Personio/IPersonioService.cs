using static Rotation.Infra.Services.Personio.PersonioServiceModels;

namespace Rotation.Infra.Services.Personio;

public interface IPersonioService
{
    Task<PersonioResponse> GetEmployeeByEmail(string email);
}
