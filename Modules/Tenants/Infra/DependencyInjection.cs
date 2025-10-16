using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Tenants.Domain;
using Modules.Tenants.Infra.Db.Contexts.TenantsDbContext;

namespace Modules.Tenants.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContext<ITenantsDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "tenants");
                optionsBuilder.MigrationsAssembly(migrationAssembly ?? typeof(DependencyInjection).Namespace);
            });
            options.UseSnakeCaseNamingConvention();
            //options.AddBaseInterceptors(serviceProvider);
        });

        return services;
    }
}
