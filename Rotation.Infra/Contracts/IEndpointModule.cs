using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Rotation.Infra.Contracts;

public interface IEndpointModule
{
    void Map(IEndpointRouteBuilder routeBuilder);
}

public static class EndpointModuleExtensions
{
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app, 
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpointModule>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints) 
            endpoint.Map(builder);

        return app;
    }
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var moduleType = typeof(IEndpointModule);

        var descriptors = assembly
            .DefinedTypes
            .Where(t => t is
            {
                IsAbstract: false,
                IsInterface: false
            } && t.IsAssignableTo(moduleType))
            .Select(s => ServiceDescriptor.Transient(moduleType, s));

        services.TryAddEnumerable(descriptors);

        return services;
    }
}
