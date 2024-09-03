using Moq;
using Rotation.Infra.Services.Slack;
using SlackNet;
using SlackNet.WebApi;

namespace Rotation.Infra.Tests.Services;

public class SlackServiceSendMessageTests
{
    [Fact]
    public async Task SendSlackMessage()
    {
        var client = new Mock<ISlackApiClient>();
        var chatApi = new Mock<IChatApi>();

        client.Setup(s => s.Chat).Returns(chatApi.Object);
        var service = new SlackService(client.Object);

        await service.SendMessageAsync(new SlackServiceModels.SlackMessage
        {
            Channel = "CBJFUV1FT",
            Text = "Hello"

        }, CancellationToken.None);

    }
}
