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
public class CurrentUserIdProviderMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserIdProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        if (Guid.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            currentUserIdProvider.CurrentUserId = userId;
        }

        if (bool.TryParse(httpContext.User.FindFirst(ImpersonationConsts.IsImpersonatedClaimKey)?.Value, out var isImpersonated))
        {
            currentUserIdProvider.IsImpersonated = isImpersonated;
        }

        if (isImpersonated)
        {
            if (Guid.TryParse(httpContext.User.FindFirst(ImpersonationConsts.PreviousUserNameIdentifierClaimKey)?.Value, out Guid previousUserUserId))
            {
                currentUserIdProvider.PreviousUserUserId = previousUserUserId;
            }

            currentUserIdProvider.PreviousUserUsername = httpContext.User.FindFirst(ImpersonationConsts.PreviousUserNameClaimKey)?.Value ?? string.Empty;

            currentUserIdProvider.PreviousUserTenantName = httpContext.User.FindFirst(ImpersonationConsts.PreviousUserTenantNameClaimKey)?.Value ?? string.Empty;
        }

        await _next(httpContext);
    }
}
