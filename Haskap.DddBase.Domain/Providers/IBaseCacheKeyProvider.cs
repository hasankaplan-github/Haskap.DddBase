using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface IBaseCacheKeyProvider
{
    string GetAllPermissionsCacheKey();
    string GetUserPermissionsCacheKey();
    string GetRolePermissionsCacheKey();
}
