using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ModuleManagement.Application.Contracts.Module;
using Modules.ModuleManagement.Domain;
using Modules.ModuleManagement.Domain.ModuleAggregate;
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

            options.UseSeeding((dbContext, _) =>
            {
                var exists = dbContext.Set<EnabledModule>().Any();

                if (exists)
                {
                    return;
                }

                var moduleService = serviceProvider.GetRequiredService<IModuleService>();

                var enabledModules = moduleService.GetModuleNames().Select(x => new EnabledModule(GuidGenerator.CreateSimpleGuid(), x)).ToList();

                dbContext.Set<EnabledModule>().AddRange(enabledModules);
                dbContext.SaveChanges();
            })
            .UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
            {
                var exists = await dbContext.Set<EnabledModule>().AnyAsync(cancellationToken);

                if (exists)
                {
                    return;
                }

                var moduleService = serviceProvider.GetRequiredService<IModuleService>();

                var enabledModules = moduleService.GetModuleNames().Select(x => new EnabledModule(GuidGenerator.CreateSimpleGuid(), x)).ToList();

                dbContext.Set<EnabledModule>().AddRange(enabledModules);
                await dbContext.SaveChangesAsync(cancellationToken);
            });
        });

        return services;
    }
}
