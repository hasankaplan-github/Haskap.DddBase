using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public interface IPermissionProvider
{
    Dictionary<string, List<PermissionRequirement>> GetAllPermissions();

    List<PermissionRequirement> GetPermissionsByGroupName(string groupName);

    void AddPermission(string groupName, string permissionName, string? displayText = null);
}