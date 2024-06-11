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
    private readonly Dictionary<string, List<PermissionRequirement>> _permissions = new();

    public BasePermissionProvider()
    {
        AddPermission(nameof(BasePermissions.App), BasePermissions.App.Admin);
        AddPermission(nameof(BasePermissions.Tenants), BasePermissions.Tenants.Admin);

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

    public void AddPermission(string groupName, string permissionName, string? displayText = null)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new ArgumentNullException(nameof(groupName));
        }

        if (string.IsNullOrWhiteSpace(permissionName))
        {
            throw new ArgumentNullException(nameof(permissionName));
        }

        if (!_permissions.TryGetValue(groupName, out var permissionRequirements))
        {
            permissionRequirements = new List<PermissionRequirement>();
            _permissions.Add(groupName, permissionRequirements);
        }

        permissionRequirements.Add(new PermissionRequirement(permissionName, displayText));
    }

    public ReadOnlyDictionary<string, List<PermissionRequirement>> GetAllPermissions()
    {
        return _permissions.AsReadOnly();
    }

    public IReadOnlyList<PermissionRequirement> GetPermissionsByGroupName(string groupName)
    {
        return _permissions.GetValueOrDefault(groupName, Enumerable.Empty<PermissionRequirement>().ToList()).AsReadOnly();
    }
}