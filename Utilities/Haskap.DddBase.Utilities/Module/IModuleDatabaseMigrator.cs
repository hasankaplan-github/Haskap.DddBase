namespace Haskap.DddBase.Utilities.Module;
public interface IModuleDatabaseMigrator
{
    Task MigrateAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}
