using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class BaseCacheKeyProvider : IBaseCacheKeyProvider
{
    protected readonly IServiceScopeFactory ServiceScopeFactory;

    public BaseCacheKeyProvider(IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
    }

    public string GetAllPermissionsCacheKey()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var currentUserIdProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserIdProvider>();

        return string.Format("AllPermissionsCacheKey_{0}", currentUserIdProvider.CurrentUserId);
    }

    public string GetRolePermissionsCacheKey()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var currentUserIdProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserIdProvider>();

        return string.Format("RolePermissionsCacheKey_{0}", currentUserIdProvider.CurrentUserId);
    }

    public string GetUserPermissionsCacheKey()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var currentUserIdProvider = scope.ServiceProvider.GetRequiredService<ICurrentUserIdProvider>();

        return string.Format("UserPermissionsCacheKey_{0}", currentUserIdProvider.CurrentUserId);
    }
}
