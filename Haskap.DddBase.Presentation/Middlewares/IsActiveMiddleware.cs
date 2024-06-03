using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation.Middlewares;
public class IsActiveMiddleware
{
    private readonly RequestDelegate _next;

    public IsActiveMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IGlobalQueryFilterGenericProvider globalQueryFilterGenericProvider,
        IIsActiveGlobalQueryFilterProvider isActiveGlobalQueryFilterProvider)
    {
        globalQueryFilterGenericProvider.AddFilterProvider<IIsActive>(isActiveGlobalQueryFilterProvider);

        await _next(httpContext);
    }
}
