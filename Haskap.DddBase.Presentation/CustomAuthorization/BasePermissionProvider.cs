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

        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.Read);
        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.ReadWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.Update);
        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.UpdateWithinSameTenant);
        AddPermission(typeof(AdminPermissions.AccountPermission), AdminPermissions.AccountPermission.UpdateWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.Read);
        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.ReadWithinDifferentTenant);
        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.Update);
        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.UpdateWithinSameTenant);
        AddPermission(typeof(AdminPermissions.AccountRole), AdminPermissions.AccountRole.UpdateWithinDifferentTenant);

        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Create);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Read);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Update);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.Delete);
        AddPermission(typeof(AdminPermissions.Role), AdminPermissions.Role.UpdatePermissions);

        AddPermission(typeof(AdminPermissions.Profile), AdminPermissions.Profile.ReadWithinSameTenant);
        AddPermission(typeof(AdminPermissions.Profile), AdminPermissions.Profile.ReadWithinDifferentTenant);

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