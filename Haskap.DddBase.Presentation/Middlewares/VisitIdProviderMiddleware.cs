using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation.Middlewares;
public class VisitIdProviderMiddleware
{
    private readonly RequestDelegate _next;

    public VisitIdProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IVisitIdProvider visitIdProvider)
    {
        visitIdProvider.Generate();

        await _next(httpContext);
    }
}
