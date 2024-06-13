using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.ObjectModel;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public interface IPermissionProvider
{
    void ConfigureAuthorization(AuthorizationOptions authorizationOptions);

    ReadOnlyDictionary<Type, List<PermissionRequirement>> GetAllPermissions();

    IReadOnlyList<PermissionRequirement> GetPermissionsByGroup(Type group);

    void AddPermission(Type group, string permissionName, string? displayText = null);
}