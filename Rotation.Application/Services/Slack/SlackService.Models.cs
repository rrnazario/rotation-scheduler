﻿namespace Rotation.Application.Services.Slack;

public static class SlackServiceModels
{
    public class SlackMessageResponse
    {
        public bool ok { get; set; }
        public string error { get; set; }
        public string channel { get; set; }
        public string ts { get; set; }
    }

    // a slack message
    public class SlackMessage
    {
        public string channel { get; set; }
        public string text { get; set; }
        public bool as_user { get; set; }
        public SlackAttachment[] attachments { get; set; }
    }

    // a slack message attachment
    public class SlackAttachment
    {
        public string fallback { get; set; }
        public string text { get; set; }
        public string image_url { get; set; }
        public string color { get; set; }
    }
}
