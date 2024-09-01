using static Rotation.Infra.Services.Slack.SlackServiceModels;

namespace Rotation.Infra.Services.Slack;

public interface ISlackService
{
    Task SendMessageAsync(SlackMessage msg, CancellationToken cancellationToken);
}


