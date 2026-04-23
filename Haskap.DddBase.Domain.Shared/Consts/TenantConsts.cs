namespace Haskap.DddBase.Domain.Shared.Consts;
public static class TenantConsts
{
    public const string IdClaimType = "tenantKey";
    public const string HeaderKey = "tenantKey";
    public const string CookieKey = "tenantKey";
    public const string QueryStringKey = "tenantKey";
    public const string NameClaimType = "tenantName";

    public const int MaxNameLength = 100;
}
