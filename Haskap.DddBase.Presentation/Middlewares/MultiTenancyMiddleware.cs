﻿using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.TenantAggregate;
using Microsoft.AspNetCore.Http;

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
        IGlobalQueryFilterParameterStatusCollectionProvider filterParameterStatusCollectionProvider,
        IMultiTenancyGlobalQueryFilterParameterStatusProvider multiTenancyGlobalQueryFilterParameterStatusProvider)
    {
        filterParameterStatusCollectionProvider.AddFilterParameterStatusProvider<IHasMultiTenant>(multiTenancyGlobalQueryFilterParameterStatusProvider);

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
                            FindFromCookie(httpContext);

        if (tenantIdString is null)
        {
            return null;
        }

        var tenantId = Guid.Parse(tenantIdString);

        return tenantId;
    }

    private string? FindFromClaims(HttpContext httpContext)
    {
        return httpContext.User.FindFirst(x => x.Type == Tenant.ClaimKey)?.Value;
    }

    private string? FindFromDomain(HttpContext httpContext)
    {
        return null;
    }

    private string? FindFromHeader(HttpContext httpContext)
    {
        return httpContext.Request.Headers[Tenant.HeaderKey].FirstOrDefault();
    }

    private string? FindFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[Tenant.CookieKey];
    }
}