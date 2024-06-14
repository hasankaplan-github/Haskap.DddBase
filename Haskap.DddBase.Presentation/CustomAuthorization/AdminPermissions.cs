namespace Haskap.DddBase.Presentation.CustomAuthorization;
public class AdminPermissions
{
    public class Tenant
    {
        public const string Create = "AdminPermissions.Tenant.Create";
        public const string Read = "AdminPermissions.Tenant.Read";
        public const string Update = "AdminPermissions.Tenant.Update";
        public const string Delete = "AdminPermissions.Tenant.Delete";
    }

    public class App
    {
        public const string Admin = "AdminPermissions.App.Admin";
        
    }

    public class Role
    {
        public const string Create = "AdminPermissions.Role.Create";
        public const string Read = "AdminPermissions.Role.Read";
        public const string Update = "AdminPermissions.Role.Update";
        public const string Delete = "AdminPermissions.Role.Delete";
        public const string UpdatePermissions = "AdminPermissions.Role.UpdatePermissions";
    }

    public class Account
    {
        public const string ReadWithinSameTenant = "AdminPermissions.Account.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.Account.ReadWithinDifferentTenant";
        public const string ImpersonateWithinSameTenant = "AdminPermissions.Account.ImpersonateWithinSameTenant";
        public const string ImpersonateWithinDifferentTenant = "AdminPermissions.Account.ImpersonateWithinDifferentTenant";
        public const string DeleteWithinSameTenant = "AdminPermissions.Account.DeleteWithinSameTenant";
        public const string DeleteWithinDifferentTenant = "AdminPermissions.Account.DeleteWithinDifferentTenant";
        //public const string UpdateOwnPermissions = "AdminPermissions.Account.UpdateOwnPermissions";
        public const string UpdatePermissionsWithinSameTenant = "AdminPermissions.Account.UpdatePermissionsWithinSameTenant";
        public const string UpdatePermissionsWithinDifferentTenant = "AdminPermissions.Account.UpdatePermissionsWithinDifferentTenant";
        //public const string UpdateOwnRoles = "AdminPermissions.Account.UpdateOwnRoles";
        public const string UpdateRolesWithinSameTenant = "AdminPermissions.Account.UpdateRolesWithinSameTenant";
        public const string UpdateRolesWithinDifferentTenant = "AdminPermissions.Account.UpdateRolesWithinDifferentTenant";
        public const string ToggleActiveStatusWithinSameTenant = "AdminPermissions.Account.ToggleActiveStatusWithinSameTenant";
        public const string ToggleActiveStatusWithinDifferentTenant = "AdminPermissions.Account.ToggleActiveStatusWithinDifferentTenant";
    }

    public class Profile
    {
        public const string ReadWithinSameTenant = "AdminPermissions.Profile.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.Profile.ReadWithinDifferentTenant";
    }
}
