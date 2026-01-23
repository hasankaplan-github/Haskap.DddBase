using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Bpm.Domain;
using Modules.Bpm.Infra.Db.Contexts.BpmDbContext;

namespace Modules.Bpm.Infra;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfra(IConfiguration configuration, string connectionStringName, string? migrationAssembly)
        {
            services.AddDbContext<IBpmDbContext, AppDbContext>((serviceProvider, options) =>
            {
                var tenantConnectionProvider = serviceProvider.GetService<ITenantConnectionStringProvider>();
                var connectionString = tenantConnectionProvider?.GetCurrentTenantConnectionString(connectionStringName) ?? configuration.GetConnectionString(connectionStringName);

                options.UseNpgsql(connectionString, optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "bpm");
                    optionsBuilder.MigrationsAssembly(migrationAssembly);
                });
                options.UseSnakeCaseNamingConvention();
                options.AddMultiTenancyInterceptors(serviceProvider);
            });

            return services;
        }
    }
}
