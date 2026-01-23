using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Domain;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDomain(IConfiguration configuration)
        {
            services.AddTransient<ProcessDomainService>();

            return services;
        }
    }
}
