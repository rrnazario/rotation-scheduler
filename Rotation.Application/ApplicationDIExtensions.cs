using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rotation.Application.Services.Slack;
using System.Net.Http.Headers;

namespace Rotation.Application;

public static class ApplicationDIExtensions
{
    public static void AddAplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISlackService, SlackService>();

        builder.Services.AddHttpClient<ISlackService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<SlackSettings>>().Value;
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
            client.BaseAddress = new Uri("https://slack.com/api");
        });
    }

    public static void UseApplication(this WebApplication app)
    {
        
    }
}

record SlackSettings
{
    public string Token { get; set; }
}
