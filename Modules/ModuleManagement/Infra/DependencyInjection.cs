using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Infra.Db.Interceptors;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Infra.Db.Contexts.ModuleManagementDbContext;
using Modules.Tenants.Infra.Db.Interceptors;

namespace Modules.ModuleManagement.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContext<IModuleManagementDbContext, AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder => 
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "module_management");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddMultiTenancyInterceptors(serviceProvider);
        });

        return services;
    }
}
