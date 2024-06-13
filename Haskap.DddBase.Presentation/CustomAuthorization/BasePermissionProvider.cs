using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public abstract class BasePermissionProvider : IPermissionProvider
{
    private readonly Dictionary<Type, List<PermissionRequirement>> _permissions = new();

    public BasePermissionProvider()
    {
        AddPermission(typeof(BasePermissions.Tenants), BasePermissions.Tenants.Admin);

        AddPermission(typeof(BasePermissions.App), BasePermissions.App.Admin);
        AddPermission(typeof(BasePermissions.App), BasePermissions.App.Impersonator);

        Define();
    }

    public abstract void Define();

    public void ConfigureAuthorization(AuthorizationOptions authorizationOptions)
    {
        var permissions = GetAllPermissions();

        foreach (var permission in permissions.SelectMany(x => x.Value))
        {
            authorizationOptions.AddPolicy(permission.Name, p => p.RequireAuthenticatedUser().AddRequirements(permission));
        }
    }

    public void AddPermission(Type group, string permissionName, string? displayText = null)
    {
        if (group is null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        if (string.IsNullOrWhiteSpace(permissionName))
        {
            throw new ArgumentNullException(nameof(permissionName));
        }

        if (!_permissions.TryGetValue(group, out var permissionRequirements))
        {
            permissionRequirements = new List<PermissionRequirement>();
            _permissions.Add(group, permissionRequirements);
        }

        permissionRequirements.Add(new PermissionRequirement(permissionName, displayText));
    }

    public ReadOnlyDictionary<Type, List<PermissionRequirement>> GetAllPermissions()
    {
        return _permissions.AsReadOnly();
    }

    public IReadOnlyList<PermissionRequirement> GetPermissionsByGroup(Type group)
    {
        return _permissions.GetValueOrDefault(group, Enumerable.Empty<PermissionRequirement>().ToList()).AsReadOnly();
    }
}