
using System.Text.Json;
using static Rotation.Infra.Services.Personio.PersonioServiceModels;

namespace Rotation.Infra.Services.Personio;

public class PersonioService
    : IPersonioService
{
    private readonly HttpClient _httpClient;

    public PersonioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<PersonioEmployeeResponse> GetEmployeeByEmail(string email, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetAsync($"company/employees?email={email}", cancellationToken);

        using var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken);
        var personioResponse = await JsonSerializer.DeserializeAsync<PersonioResponse>(responseStream, cancellationToken: cancellationToken);

        return PersonioEmployeeResponse.Parse(personioResponse!);
    }
}
