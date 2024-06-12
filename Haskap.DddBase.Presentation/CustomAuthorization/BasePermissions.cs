namespace Haskap.DddBase.Presentation.CustomAuthorization;
public class BasePermissions
{
    public static class Tenants
    {
        public const string Admin = "Permissions.Tenants.Admin";
    }

    public static class App
    {
        public const string Admin = "Permissions.App.Admin";
        public const string Impersonation = "Permissions.App.Impersonation";
    }
}
