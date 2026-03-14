using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Interceptors;
using Haskap.DddBase.Infra.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Domain.ModuleAggregate;
using Modules.ModuleManagement.Infra.Db.Contexts.ModuleManagementDbContext;

namespace Modules.ModuleManagement.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        services.AddDbContextFactory<AppDbContext>((serviceProvider, options) =>
        {
            var tenantConnectionProvider = serviceProvider.GetService<ITenantConnectionStringProvider>();
            var connectionString = tenantConnectionProvider?.GetCurrentTenantConnectionString(connectionStringName) ?? configuration.GetConnectionString(connectionStringName);

            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "module_management");
                optionsBuilder.MigrationsAssembly(migrationAssembly);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddMultiTenancyInterceptors(serviceProvider);

            options.UseSeeding((dbContext, _) =>
            {
                var currentTenantProvider = serviceProvider.GetRequiredService<ICurrentTenantProvider>();
                if (!currentTenantProvider.IsHost)
                {
                    return;
                }

                //var exists = dbContext.Set<EnabledModule>().Where(x => x.TenantId == null).Any();
                var exists = dbContext.Set<EnabledModule>().Any();

                if (exists)
                {
                    return;
                }

                var moduleService = serviceProvider.GetRequiredService<IModuleService>();

                var enabledModules = moduleService.GetModuleNamesRegisteredInSystem().Select(x => new EnabledModule(GuidGenerator.CreateSimpleGuid(), x)).ToList();

                dbContext.Set<EnabledModule>().AddRange(enabledModules);
                dbContext.SaveChanges();
            })
            .UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
            {
                var currentTenantProvider = serviceProvider.GetRequiredService<ICurrentTenantProvider>();
                if (!currentTenantProvider.IsHost)
                {
                    return;
                }

                //var exists = await dbContext.Set<EnabledModule>().Where(x => x.TenantId == null).AnyAsync(cancellationToken);
                var exists = await dbContext.Set<EnabledModule>().AnyAsync(cancellationToken);

                if (exists)
                {
                    return;
                }

                var moduleService = serviceProvider.GetRequiredService<IModuleService>();

                var enabledModules = moduleService.GetModuleNamesRegisteredInSystem().Select(x => new EnabledModule(GuidGenerator.CreateSimpleGuid(), x)).ToList();

                dbContext.Set<EnabledModule>().AddRange(enabledModules);
                await dbContext.SaveChangesAsync(cancellationToken);
            });
        }, ServiceLifetime.Scoped);
        services.AddScoped<IMyDbContextFactory<IModuleManagementDbContext>, MyDbContextFactory<IModuleManagementDbContext, AppDbContext>>();
        services.AddScoped<IModuleManagementDbContext, AppDbContext>();

        return services;
    }
}
