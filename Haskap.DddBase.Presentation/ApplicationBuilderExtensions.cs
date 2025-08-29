using Haskap.DddBase.Utilities.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Presentation;
public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> MigrateModulesDatabasesAsync(this IApplicationBuilder builder, CancellationToken cancellationToken = default)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();

        var moduleDatabaseMigrators = serviceScope.ServiceProvider.GetServices<IModuleDatabaseMigrator>();

        foreach (var moduleDatabaseMigrator in moduleDatabaseMigrators)
        {
            await moduleDatabaseMigrator.MigrateAsync(serviceScope, cancellationToken);
        }

        return builder;
    }
}
