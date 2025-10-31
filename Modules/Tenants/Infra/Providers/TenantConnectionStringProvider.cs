using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared;
using Haskap.DddBase.Domain.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Modules.Tenants.Application.Contracts.Tenants;
using Modules.Tenants.Domain.Providers;

namespace Modules.Tenants.Infra.Providers;

public class TenantConnectionStringProvider : ITenantConnectionStringProvider
{
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly IConfiguration _configuration;
    private readonly ITenantService _tenantService;

    public TenantConnectionStringProvider(
        ICurrentTenantProvider currentTenantProvider,
        IConfiguration configuration,
        ITenantService tenantService)
    {
        _currentTenantProvider = currentTenantProvider;
        _configuration = configuration;
        _tenantService = tenantService;
    }

    public string GetCurrentTenantConnectionString(string configurationHostConnectionStringName)
    {
        if (AppConfig.IsMultiTenant)
        {
            if (AppConfig.MultiTenancyType == MultiTenancyType.SharedDb)
            {
                return _configuration.GetConnectionString(configurationHostConnectionStringName)!;
            }

            if (AppConfig.MultiTenancyType == MultiTenancyType.DbPerTenant)
            {
                if (_currentTenantProvider.IsHost)
                {
                    return _configuration.GetConnectionString(configurationHostConnectionStringName)!;
                }

                var tenant = _tenantService.GetByIdAsync(_currentTenantProvider.CurrentTenantId.Value, default).GetAwaiter().GetResult();
                return tenant.ConnectionString;
            }
        }

        return _configuration.GetConnectionString(configurationHostConnectionStringName)!;
    }
}
