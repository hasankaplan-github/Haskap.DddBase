using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Haskap.DddBase.Presentation;

public class UtilityMethods
{
    public static bool IsAjaxRequest(HttpRequest request)
    {
        return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
    }

    public static Guid? FindTenant(HttpContext httpContext)
    {
        var tenantIdString = FindFromClaims(httpContext) ??
                            FindFromDomain(httpContext) ??
                            FindFromHeader(httpContext) ??
                            FindFromCookie(httpContext) ??
                            FromQueryString(httpContext);

        if (Guid.TryParse(tenantIdString, out Guid tenantId))
        {
            return tenantId;
        }

        return null;
    }

    private static string? FindFromClaims(HttpContext httpContext)
    {
        return httpContext.User.FindFirst(x => x.Type == TenantConsts.IdClaimType)?.Value;
    }

    private static string? FindFromDomain(HttpContext httpContext)
    {
        return null;
    }

    private static string? FindFromHeader(HttpContext httpContext)
    {
        var tenantId = httpContext.Request.Headers[TenantConsts.HeaderKey].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        return tenantId;
    }

    private static string? FindFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[TenantConsts.CookieKey];
    }

    private static string? FromQueryString(HttpContext httpContext)
    {
        var tenantId = httpContext.Request?.Query[TenantConsts.QueryStringKey].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        return tenantId;
    }
}
