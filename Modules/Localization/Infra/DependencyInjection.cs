using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Localization.Domain;
using Modules.Localization.Infra.Db.Contexts.LocalizationDbContext;

namespace Modules.Localization.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContextFactory<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder => 
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "localization");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            //options.AddMultiTenancyInterceptors(serviceProvider);
        }, ServiceLifetime.Scoped);
        services.AddScoped<IMyDbContextFactory<ILocalizationDbContext>, MyDbContextFactory<ILocalizationDbContext, AppDbContext>>();
        services.AddScoped<ILocalizationDbContext, AppDbContext>();

        return services;
    }
}
