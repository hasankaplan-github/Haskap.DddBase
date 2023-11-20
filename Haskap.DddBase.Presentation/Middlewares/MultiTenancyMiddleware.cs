using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Haskap.DddBase.Presentation.Middlewares;

public class MultiTenancyMiddleware
{
    private readonly RequestDelegate _next;

    public MultiTenancyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IMultiTenancyGlobalQueryFilterProvider multiTenancyGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<IHasMultiTenant>(multiTenancyGlobalQueryFilterProvider);

        using (currentTenantProvider.ChangeCurrentTenant(FindTenant(httpContext)))
        {
            await _next(httpContext);
        }
    }

    private Guid? FindTenant(HttpContext httpContext)
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

    private string? FindFromClaims(HttpContext httpContext)
    {
        return httpContext.User.FindFirst(x => x.Type == TenantConsts.ClaimKeyClaimType)?.Value;
    }

    private string? FindFromDomain(HttpContext httpContext)
    {
        return null;
    }

    private string? FindFromHeader(HttpContext httpContext)
    {
        var tenantId = httpContext.Request.Headers[TenantConsts.HeaderKey].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantId)) 
        {
            return null;
        }

        return tenantId;
    }

    private string? FindFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[TenantConsts.CookieKey];
    }

    private string? FromQueryString(HttpContext httpContext)
    {
        var tenantId = httpContext.Request?.Query[TenantConsts.QueryStringKey].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        return tenantId;
    }
}
