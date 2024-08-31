﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rotation.Domain.Activities;
using Rotation.Domain.SeedWork;
using Rotation.Domain.Users;
using Rotation.Infra.Activities;
using Rotation.Infra.Persistence;
using Rotation.Infra.Users;

namespace Rotation.Infra;

public static class InfraDIExtensions
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        RegisterRepositories(builder);
    }

    private static void RegisterRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }
}
