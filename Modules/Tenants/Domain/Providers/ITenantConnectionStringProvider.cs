namespace Modules.Tenants.Domain.Providers;

public interface ITenantConnectionStringProvider
{
    string GetCurrentTenantConnectionString(string configurationConnectionStringName);
}
