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

        if (tenantIdString is null)
        {
            return null;
        }

        var tenantId = Guid.Parse(tenantIdString);

        return tenantId;
    }

    private string? FindFromClaims(HttpContext httpContext)
    {
        var tenantId = httpContext.User.FindFirst(x => x.Type == TenantConsts.ClaimKey)?.Value;

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            return null;
        }

        return tenantId;
    }

    private string? FindFromDomain(HttpContext httpContext)
    {
        return null;
    }

    private string? FindFromHeader(HttpContext httpContext)
    {
        return httpContext.Request.Headers[TenantConsts.HeaderKey].FirstOrDefault();
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
