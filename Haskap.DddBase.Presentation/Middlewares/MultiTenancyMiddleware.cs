using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
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
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IMultiTenancyGlobalQueryFilterProvider multiTenancyGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<IHasMultiTenant>(multiTenancyGlobalQueryFilterProvider);

        using (currentTenantProvider.ChangeCurrentTenant(httpContext.FindTenantId()))
        {
            await _next(httpContext);
        }
    }
}
