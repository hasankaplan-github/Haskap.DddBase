namespace Haskap.DddBase.Domain.Providers;

public interface ITenantConnectionStringProvider
{
    string GetCurrentTenantConnectionString(string configurationConnectionStringName);
}
