using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rotation.Application.Services.Personio;
using Rotation.Application.Services.Slack;
using System.Net.Http.Headers;

namespace Rotation.Application;

public static class ApplicationDIExtensions
{
    public static void AddAplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISlackService, SlackService>();
        builder.Services.AddScoped<IPersonioService, PersonioService>();

        builder.Services.AddHttpClient<ISlackService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<SlackSettings>>().Value;
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
            client.BaseAddress = new Uri("https://slack.com/api");
        });

        builder.Services.AddHttpClient<IPersonioService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<PersonioSettings>>().Value;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
            client.DefaultRequestHeaders.Add("X-Personio-Partner-ID", settings.PartnerId);
            client.DefaultRequestHeaders.Add("X-Personio-App-ID", settings.AppId);
            client.BaseAddress = new Uri("https://api.personio.de/v1");
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

record PersonioSettings
{
    public string Token { get; set; }
    public string PartnerId { get; set; }
    public string AppId { get; set; }
}
