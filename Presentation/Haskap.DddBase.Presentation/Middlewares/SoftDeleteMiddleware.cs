using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Http;

namespace Haskap.DddBase.Presentation.Middlewares;
public class SoftDeleteMiddleware
{
    private readonly RequestDelegate _next;

    public SoftDeleteMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext, 
        IGlobalQueryFilterManagerProvider globalQueryFilterGenericProvider,
        ISoftDeleteGlobalQueryFilterProvider softDeleteGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<ISoftDeletable>(softDeleteGlobalQueryFilterProvider);

        await _next(httpContext);
    }
}
