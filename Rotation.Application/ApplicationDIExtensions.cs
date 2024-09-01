using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rotation.Infra.Services.Personio;
using Rotation.Infra.Services.Slack;
using System.Net.Http.Headers;

namespace Rotation.Infra;

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
