using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Collections.ObjectModel;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public interface IPermissionProvider
{
    void ConfigureAuthorization(AuthorizationOptions authorizationOptions);

    ReadOnlyDictionary<string, List<PermissionRequirement>> GetAllPermissions();

    IReadOnlyList<PermissionRequirement> GetPermissionsByGroupName(string groupName);

    void AddPermission(string groupName, string permissionName, string? displayText = null);
}