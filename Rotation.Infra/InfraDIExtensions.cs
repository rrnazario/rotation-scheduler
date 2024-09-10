using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rotation.Infra.Services.Personio;
using Rotation.Infra.Services.Slack;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Activities;
using Rotation.Infra.Persistence;
using Rotation.Infra.Users;
using SlackNet.AspNetCore;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Rotation.Infra;

public static class InfraDIExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ISlackService, SlackService>();
        builder.Services.AddSingleton<IPersonioTokenHandler, PersonioTokenHandler>();
        var personioSettings = builder.Configuration.GetSection("Personio").Get<PersonioSettings>()!;
        builder.Services.AddSingleton(personioSettings);

        builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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
        var slackSettings = builder.Configuration.GetSection("Slack").Get<SlackSettings>()!;

        builder.Services.AddSlackNet(c => c.UseApiToken(slackSettings.Token));

        builder.Services.AddHttpClient<IPersonioService>((_, client) =>
        {
            client.BaseAddress = new Uri("https://api.personio.de/v1");
        });
    }
}

record SlackSettings
{
    public string Token { get; set; }
}

public record PersonioSettings
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}