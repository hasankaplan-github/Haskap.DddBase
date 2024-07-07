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

    public class AccountPermission
    {
        public const string Read = "AdminPermissions.AccountPermission.Read";
        public const string ReadWithinSameTenant = "AdminPermissions.AccountPermission.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.AccountPermission.ReadWithinDifferentTenant";
        public const string Update = "AdminPermissions.AccountPermission.Update";
        public const string UpdateWithinSameTenant = "AdminPermissions.AccountPermission.UpdateWithinSameTenant";
        public const string UpdateWithinDifferentTenant = "AdminPermissions.AccountPermission.UpdateWithinDifferentTenant";
    }

    public class AccountRole
    {
        public const string Read = "AdminPermissions.AccountRole.Read";
        public const string ReadWithinSameTenant = "AdminPermissions.AccountRole.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.AccountRole.ReadWithinDifferentTenant";
        public const string Update = "AdminPermissions.AccountRole.Update";
        public const string UpdateWithinSameTenant = "AdminPermissions.AccountRole.UpdateWithinSameTenant";
        public const string UpdateWithinDifferentTenant = "AdminPermissions.AccountRole.UpdateWithinDifferentTenant";
    }

    public class Account
    {
        public const string ReadWithinSameTenant = "AdminPermissions.Account.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.Account.ReadWithinDifferentTenant";
        public const string ImpersonateWithinSameTenant = "AdminPermissions.Account.ImpersonateWithinSameTenant";
        public const string ImpersonateWithinDifferentTenant = "AdminPermissions.Account.ImpersonateWithinDifferentTenant";
        public const string DeleteWithinSameTenant = "AdminPermissions.Account.DeleteWithinSameTenant";
        public const string DeleteWithinDifferentTenant = "AdminPermissions.Account.DeleteWithinDifferentTenant";
        public const string ToggleActiveStatusWithinSameTenant = "AdminPermissions.Account.ToggleActiveStatusWithinSameTenant";
        public const string ToggleActiveStatusWithinDifferentTenant = "AdminPermissions.Account.ToggleActiveStatusWithinDifferentTenant";
    }

    public class Profile
    {
        public const string ReadWithinSameTenant = "AdminPermissions.Profile.ReadWithinSameTenant";
        public const string ReadWithinDifferentTenant = "AdminPermissions.Profile.ReadWithinDifferentTenant";
    }
}
