using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rotation.Infra.Services.Personio;
using Rotation.Infra.Services.Slack;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Activities;
using Rotation.Infra.Persistence;
using Rotation.Infra.Users;
using System.Net.Http.Headers;

namespace Rotation.Infra;

public static class InfraDIExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ISlackService, SlackService>();
        builder.Services.AddScoped<IPersonioService, PersonioService>();

        RegisterRepositories(builder);
        RegisterHttpClients(builder);
    }

    private static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void RegisterHttpClients(WebApplicationBuilder builder)
    {
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