using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public interface IModuleDatabaseMigrator
{
    Task MigrateAsync(IServiceScope scope, CancellationToken cancellationToken = default);
}
