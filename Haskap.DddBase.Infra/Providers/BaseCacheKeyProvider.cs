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
    public string GetAllPermissionsCacheKey(Guid userId)
    {
        return string.Format("AllPermissionsCacheKey_{0}", userId);
    }

    public string GetRolePermissionsCacheKey(Guid roleId)
    {
        return string.Format("RolePermissionsCacheKey_{0}", roleId);
    }

    public string GetUserPermissionsCacheKey(Guid userId)
    {
        return string.Format("UserPermissionsCacheKey_{0}", userId);
    }
}
