using Haskap.DddBase.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Haskap.DddBase.Presentation.CustomAuthorization;
public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            var failedPermissions = authorizeResult.AuthorizationFailure?.FailureReasons
                .Where(x => x.Handler is PermissionAuthorizationHandler)
                .Select(x => x.Message);

            if (failedPermissions?.Any(x => !string.IsNullOrWhiteSpace(x)) == true)
            {
                throw new ForbiddenOperationException(string.Join(',', failedPermissions));
            }
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
