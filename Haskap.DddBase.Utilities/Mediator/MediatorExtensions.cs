using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Haskap.DddBase.Utilities.Mediator;
public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorConsumersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var registrar = new DependencyInjectionMediatorContainerRegistrar(services);

        assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>)))
            .ToList()
            .ForEach(consumerType =>
            {
                services.RegisterConsumer(registrar, consumerType);
            });

        return services;
    }
}
