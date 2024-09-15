using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Rotation.Infra.Services.Personio.Models.PersonioEmployeeModels;
using static Rotation.Infra.Services.Personio.Models.PersonioModels;
using static Rotation.Infra.Services.Personio.Models.PersonioTimeOffModels;

namespace Rotation.Infra.Services.Personio;

public class PersonioClient
    : IPersonioClient
{
    private readonly HttpClient _httpClient;
    private readonly IPersonioTokenHandler _tokenHandler;
    private readonly PersonioSettings _personioSettings;

    public PersonioClient(
        HttpClient httpClient, 
        IPersonioTokenHandler personioTokenHandler,
        PersonioSettings personioSettings)
    {
        _httpClient = httpClient;
        _tokenHandler = personioTokenHandler;
        _personioSettings = personioSettings;
    }

    public async Task<PersonioEmployeeResponse> GetEmployeeByEmail(string email, CancellationToken cancellationToken)
    {
        var personioResponse = await PerformRequest<PersonioResponse<PersonioEmployeeAttribute>>(
            HttpMethod.Get,
            $"company/employees?email={email}", cancellation: cancellationToken);

        return PersonioEmployeeResponse.Parse(personioResponse!);
    }

    public async Task<PersonioTimeOffResponse> GetTimeOffAsync(GetTimeOffAsyncRequest request,
        CancellationToken cancellationToken)
    {
        var personioResponse = await PerformRequest<PersonioResponse<dynamic>>(HttpMethod.Get,
            $"company/time-offs{request.ToParams()}", cancellation: cancellationToken);

        return PersonioTimeOffResponse.Parse(personioResponse!);
    }

    private async Task<TResponse?> PerformRequest<TResponse>(HttpMethod method, string url,
        bool isFinalRetry = false, CancellationToken cancellation = default)
    {
        try
        {
            using var message = new HttpRequestMessage(method, url);
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenHandler.GetToken());

            using var response = await _httpClient.SendAsync(message, cancellation);

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellation);
            return await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                cancellationToken: cancellation);
        }
        catch (UnauthorizedAccessException)
        {
            if (isFinalRetry) throw;

            await Authenticate(cancellation);

            return await PerformRequest<TResponse>(method, url, isFinalRetry: true, cancellation);
        }
    }

    private async Task Authenticate(CancellationToken cancellationToken)
    {
        var content = new
        {
            client_id = _personioSettings.ClientId,
            client_secret = _personioSettings.ClientSecret,
        };

        var response = await _httpClient.PostAsync("auth",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"), cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var s = await JsonSerializer.DeserializeAsync<dynamic>(responseStream,
            cancellationToken: cancellationToken);

        _tokenHandler.SetToken(s!.data.token);
    }
}