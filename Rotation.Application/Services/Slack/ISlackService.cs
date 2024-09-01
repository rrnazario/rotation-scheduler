using static Rotation.Application.Services.Slack.SlackServiceModels;

namespace Rotation.Application.Services.Slack;

public interface ISlackService
{
    Task SendMessageAsync(SlackMessage msg, CancellationToken cancellationToken);
}


