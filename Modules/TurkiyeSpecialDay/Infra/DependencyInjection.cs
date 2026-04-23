using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.SpecialDayBase.Domain.Providers;
using Modules.TurkiyeSpecialDay.Infra.Providers;

namespace Modules.TurkiyeSpecialDay.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        services.AddScoped<ISpecialDayCalendarProvider, TurkiyeSpecialDayCalendarProvider>();

        return services;
    }
}
