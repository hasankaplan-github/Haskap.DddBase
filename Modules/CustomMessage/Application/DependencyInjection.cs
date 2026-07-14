using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.CustomMessage.Application.Contracts;

namespace Modules.CustomMessage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomMessageService, CustomMessageService>();

        return services;
    }
}
