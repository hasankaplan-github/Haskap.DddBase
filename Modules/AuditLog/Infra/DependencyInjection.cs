using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.AuditLog.Domain;
using Modules.AuditLog.Infra.Db.Contexts.AuditLogDbContext;
using Modules.AuditLog.Infra.Db.Interceptors;

namespace Modules.AuditLog.Infra;
public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration, string connectionStringName, string? migrationAssembly)
    {
        services.AddScoped<AuditHistoryLogSaveChangesInterceptor>();
        services.AddScoped<AuditSaveChangesInterceptor>();

        services.AddScoped<IAuditLogDbContext, AppDbContext>();

        var connectionString = configuration.GetConnectionString(connectionStringName);
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "audit_log");
                optionsBuilder.MigrationsAssembly(migrationAssembly ?? typeof(DependencyInjection).Namespace);
            });
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
