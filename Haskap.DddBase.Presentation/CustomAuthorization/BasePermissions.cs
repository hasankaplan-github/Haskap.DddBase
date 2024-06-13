namespace Haskap.DddBase.Presentation.CustomAuthorization;
public class BasePermissions
{
    public class Tenants
    {
        public const string Admin = "BasePermissions.Tenants.Admin";
        public const string RoleEditor = "BasePermissions.Tenants.RoleEditor";
    }

    public class App
    {
        public const string Admin = "BasePermissions.App.Admin";
        public const string Impersonator = "BasePermissions.App.Impersonator";
    }
}
