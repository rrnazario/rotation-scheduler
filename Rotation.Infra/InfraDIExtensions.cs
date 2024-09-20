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
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        RegisterDatabase(builder);
        RegisterRepositories(builder);
        RegisterHttpClients(builder);
    }

    private static void RegisterDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(ctx =>
        {
            var options = builder.Configuration
                .GetSection("Database")
                .Get<EFPersistenceOptions>()!;

            var dataSource = new NpgsqlDataSourceBuilder(options.ConnectionString)
                .EnableDynamicJson()
                .Build();

            ctx.UseNpgsql(dataSource, action =>
            {
                action.EnableRetryOnFailure(options.MaxRetryCount);
                action.CommandTimeout(options.CommandTimeout);
            });

            ctx.EnableDetailedErrors(options.EnableDetailedErrors);
            ctx.EnableSensitiveDataLogging(options.EnableSensitiveDataLogging);
        });
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

        builder.Services.AddHttpClient<IPersonioClient, PersonioClient>((sp, client) =>
        {
            client.BaseAddress = new Uri("https://api.personio.de/v1/");
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

public record EFPersistenceOptions(
    string ConnectionString,
    int MaxRetryCount = 5,
    int CommandTimeout = 100,
    bool EnableDetailedErrors = false,
    bool EnableSensitiveDataLogging = false);