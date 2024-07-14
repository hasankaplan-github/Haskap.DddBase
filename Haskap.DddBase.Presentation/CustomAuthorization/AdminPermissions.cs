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
        public const string ToggleActiveStatusWithinSameTenant = "AdminPermissions.Account.ToggleActiveStatusWithinSameTenant";
        public const string ToggleActiveStatusWithinDifferentTenant = "AdminPermissions.Account.ToggleActiveStatusWithinDifferentTenant";

        public class Permission
        {
            public const string Read = "AdminPermissions.Account.Permission.Read";
            public const string ReadWithinSameTenant = "AdminPermissions.Account.Permission.ReadWithinSameTenant";
            public const string ReadWithinDifferentTenant = "AdminPermissions.Account.Permission.ReadWithinDifferentTenant";
            public const string Update = "AdminPermissions.Account.Permission.Update";
            public const string UpdateWithinSameTenant = "AdminPermissions.Account.Permission.UpdateWithinSameTenant";
            public const string UpdateWithinDifferentTenant = "AdminPermissions.Account.Permission.UpdateWithinDifferentTenant";
        }

        public class Role
        {
            public const string Read = "AdminPermissions.Account.Role.Read";
            public const string ReadWithinSameTenant = "AdminPermissions.Account.Role.ReadWithinSameTenant";
            public const string ReadWithinDifferentTenant = "AdminPermissions.Account.Role.ReadWithinDifferentTenant";
            public const string Update = "AdminPermissions.Account.Role.Update";
            public const string UpdateWithinSameTenant = "AdminPermissions.Account.Role.UpdateWithinSameTenant";
            public const string UpdateWithinDifferentTenant = "AdminPermissions.Account.Role.UpdateWithinDifferentTenant";
        }

        public class Profile
        {
            public const string ReadWithinSameTenant = "AdminPermissions.Account.Profile.ReadWithinSameTenant";
            public const string ReadWithinDifferentTenant = "AdminPermissions.Account.Profile.ReadWithinDifferentTenant";
        }
    }
}
