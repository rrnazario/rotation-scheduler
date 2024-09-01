using System.Text;
using System.Text.Json;
using static Rotation.Application.Services.Slack.SlackServiceModels;

namespace Rotation.Application.Services.Slack;

public class SlackService
    : ISlackService
{
    private readonly HttpClient _client;

    public SlackService(HttpClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync(SlackMessage msg, CancellationToken cancellationToken)
    {
        var content = JsonSerializer.Serialize(msg);

        var httpContent = new StringContent(
            content,
            Encoding.UTF8,
            "application/json"
        );

        var messageResponse = await GetSlackResponseAsync(httpContent, cancellationToken);

        if (!messageResponse.ok)
        {
            throw new Exception(
                "failed to send message. error: " + messageResponse.error
            );
        }
    }

    private async Task<SlackMessageResponse> GetSlackResponseAsync(StringContent httpContent, CancellationToken cancellationToken)
    {
        var response = await _client.PostAsync("chat.postMessage", httpContent, cancellationToken);
        using var responseJson = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<SlackMessageResponse>(responseJson, cancellationToken: cancellationToken);
    }
}


