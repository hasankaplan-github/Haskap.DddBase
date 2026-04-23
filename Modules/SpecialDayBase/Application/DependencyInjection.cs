using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.SpecialDayBase.Application.Contracts;

namespace Modules.SpecialDayBase.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ISpecialDayService, SpecialDayService>();
        services.AddKeyedSingleton<ISpecialDaySpecificationEvaluator, FixedAndOccurrenceBasedSpecialDaySpecificationEvaluator>("FixedAndOccurrenceBased");
        services.AddKeyedSingleton<ISpecialDaySpecificationEvaluator, FixedWithExactDateSpecialDaySpecificationEvaluator>("FixedWithExactDate");
        services.AddKeyedSingleton<ISpecialDaySpecificationEvaluator, OneTimeAndOccurrenceBasedSpecialDaySpecificationEvaluator>("OneTimeAndOccurrenceBased");
        services.AddKeyedSingleton<ISpecialDaySpecificationEvaluator, OneTimeWithExactDateSpecialDaySpecificationEvaluator>("OneTimeWithExactDate");

        return services;
    }
}
