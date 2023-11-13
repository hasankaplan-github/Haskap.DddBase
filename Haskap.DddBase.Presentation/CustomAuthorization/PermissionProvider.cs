using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation.CustomAuthorization;
//basede
public abstract class PermissionProvider : IPermissionProvider
{
    private readonly Dictionary<string, List<PermissionRequirement>> _permissions = new();

    public PermissionProvider()
    {
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

        var permissions = _permissions.GetValueOrDefault(groupName, new List<PermissionRequirement>());
        permissions.Add(new PermissionRequirement(permissionName, displayText));

        _permissions.Remove(groupName);
        _permissions.Add(groupName, permissions);
    }

    public Dictionary<string, List<PermissionRequirement>> GetAllPermissions()
    {
        return _permissions;
    }

    public List<PermissionRequirement> GetPermissionsByGroupName(string groupName)
    {
        return _permissions.GetValueOrDefault(groupName, new List<PermissionRequirement>());
    }
}