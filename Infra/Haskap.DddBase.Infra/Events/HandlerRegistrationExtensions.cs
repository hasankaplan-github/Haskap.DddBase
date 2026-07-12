using Haskap.DddBase.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Haskap.DddBase.Infra.Events;
public static class HandlerRegistrationExtensions
{
    public static IServiceCollection RegisterHandlersFromAssembly(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            RegisterEventHandlers(services, assembly);

        return services;
    }

    private static void RegisterEventHandlers(IServiceCollection services, Assembly assembly)
    {
        var eventHandlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.IsAssignableTo(typeof(IEventHandler)))
            .ToList();

        foreach (var implementationType in eventHandlerTypes)
        {
            // Find all IEventHandler<T> interfaces implemented by this type
            var handlerInterfaces = implementationType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            foreach (var interfaceType in handlerInterfaces)
            {
                // Register as both the specific IEventHandler<T> and the non-generic IEventHandler
                services.AddTransient(interfaceType, implementationType);
            }
        }
    }
}
