using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Modules.ViewLevelExceptions.Domain;
using Modules.ViewLevelExceptions.Infra.Db.Contexts.ViewLevelExceptionsDbContext;

namespace Modules.ViewLevelExceptions.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        //services.AddBaseDbContext(typeof(ITenantsDbContext), typeof(AppDbContext));
        services.AddScoped<IViewLevelExceptionsDbContext, AppDbContext>();

        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "view_level_exceptions");
                optionsBuilder.MigrationsAssembly(migrationAssembly ?? typeof(DependencyInjection).Namespace);
            });
            options.UseSnakeCaseNamingConvention();
            //options.AddBaseInterceptors(serviceProvider);
        });

        return services;
    }
}
