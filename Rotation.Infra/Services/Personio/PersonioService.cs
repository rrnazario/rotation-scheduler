
using System.Text.Json;
using static Rotation.Infra.Services.Personio.Models.PersonioEmployeeModels;
using static Rotation.Infra.Services.Personio.Models.PersonioModels;
using static Rotation.Infra.Services.Personio.Models.PersonioTimeOffModels;

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
        var personioResponse = await JsonSerializer.DeserializeAsync<PersonioResponse<PersonioEmployeeAttribute>>(responseStream, cancellationToken: cancellationToken);

        return PersonioEmployeeResponse.Parse(personioResponse!);
    }

    public async Task<PersonioTimeOffResponse> GetTimeOffAsync(GetTimeOffAsyncRequest request, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetAsync($"company/time-offs{request.ToParams()}", cancellationToken);

        using var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken);
        var personioResponse = await JsonSerializer.DeserializeAsync<PersonioResponse<dynamic>>(responseStream, cancellationToken: cancellationToken);

        return PersonioTimeOffResponse.Parse(personioResponse!);
    }
}
