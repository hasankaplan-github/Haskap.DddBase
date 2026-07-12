using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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
        if (Guid.TryParse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid currentUserId))
        {
            currentUserIdProvider.CurrentUserId = currentUserId;
        }

        if (bool.TryParse(httpContext.User.FindFirstValue(ImpersonationConsts.IsImpersonatedClaimKey), out var isImpersonated))
        {
            currentUserIdProvider.IsImpersonated = isImpersonated;
        }

        if (isImpersonated)
        {
            if (Guid.TryParse(httpContext.User.FindFirstValue(ImpersonationConsts.ImpersonatorNameIdentifierClaimKey), out Guid impersonatorUserId))
            {
                currentUserIdProvider.ImpersonatorUserId = impersonatorUserId;
            }

            currentUserIdProvider.ImpersonatorUsername = httpContext.User.FindFirstValue(ImpersonationConsts.ImpersonatorNameClaimKey)!;

            currentUserIdProvider.ImpersonatorTenantName = httpContext.User.FindFirstValue(ImpersonationConsts.ImpersonatorTenantNameClaimKey)!;
        }

        if (Guid.TryParse(httpContext.User.FindFirstValue(AccountConsts.LoginIdClaimType), out Guid currentLoginId))
        {
            currentUserIdProvider.CurrentLoginId = currentLoginId;
        }

        await _next(httpContext);
    }
}
