using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public interface IPermissionProvider
{
    Dictionary<string, List<OperationAuthorizationRequirement>> GetAllPermissions();

    List<OperationAuthorizationRequirement> GetPermissionsByGroupName(string groupName);

    void AddPermission(string groupName, string permissionName);
}