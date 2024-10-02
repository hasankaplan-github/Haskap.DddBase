using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
using Haskap.DddBase.Modules.Tenants.Domain;
using Haskap.DddBase.Modules.Tenants.Infra.Db.Contexts.TenantsDbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Haskap.DddBase.Modules.Tenants.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
    {
        //services.AddBaseDbContext(typeof(ITenantsDbContext), typeof(AppDbContext));
        services.AddScoped<ITenantsDbContext, AppDbContext>();

        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder => optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "tenants"));
            options.UseSnakeCaseNamingConvention();
            //options.AddBaseInterceptors(serviceProvider);
        });

        return services;
    }
}
