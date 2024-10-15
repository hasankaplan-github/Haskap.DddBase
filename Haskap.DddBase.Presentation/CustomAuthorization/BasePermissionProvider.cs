using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public abstract class BasePermissionProvider : IPermissionProvider
{
    private readonly Dictionary<string, HashSet<PermissionRequirement>> _permissions = new();

    public BasePermissionProvider()
    {
        AddPermission(typeof(AdminPermissions.App), AdminPermissions.App.Admin);

        AddPermission(typeof(AdminPermissions.Tenant), AdminPermissions.Tenant.Create);
        AddPermission(typeof(AdminPermissions.Tenant), AdminPermissions.Tenant.Read);
        AddPermission(typeof(AdminPermissions.Tenant), AdminPermissions.Tenant.Update);
        AddPermission(typeof(AdminPermissions.Tenant), AdminPermissions.Tenant.Delete);

        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ReadWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ImpersonateWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ImpersonateWithinDifferentTenant); 
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.DeleteWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.DeleteWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ToggleActiveStatusWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ToggleActiveStatusWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.Account.Profile), AdminPermissions.Account.Profile.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account.Profile), AdminPermissions.Account.Profile.ReadWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.Read);
        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.ReadWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.Update);
        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.UpdateWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account.Permission), AdminPermissions.Account.Permission.UpdateWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.Read);
        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.ReadWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.Update);
        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.UpdateWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Account.Role), AdminPermissions.Account.Role.UpdateWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Create);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Read);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Update);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Delete);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.UpdatePermissions);

        
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

    protected void AddPermission(Type group, string permissionName, string? displayText = null)
    {
        if (group is null)
        {
            throw new ArgumentNullException(nameof(group));
        }

        if (string.IsNullOrWhiteSpace(permissionName))
        {
            throw new ArgumentNullException(nameof(permissionName));
        }

        var key = group.FullName!.Substring(group.Namespace!.Length + 1).Replace('+', '.');

        if (!_permissions.TryGetValue(key, out var permissionRequirements))
        {
            permissionRequirements = new HashSet<PermissionRequirement>(new PermissionRequirementEqualityComparer());
            _permissions.Add(key, permissionRequirements);
        }

        permissionRequirements.Add(new PermissionRequirement(permissionName, displayText));
    }

    public ReadOnlyDictionary<string, HashSet<PermissionRequirement>> GetAllPermissions()
    {
        return _permissions.AsReadOnly();
    }

    public IReadOnlySet<PermissionRequirement> GetPermissionsByGroup(Type group)
    {
        var key = group.FullName!.Substring(group.Namespace!.Length + 1).Replace('+', '.');

        return _permissions.GetValueOrDefault(key, Enumerable.Empty<PermissionRequirement>().ToHashSet());
    }
}