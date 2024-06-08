using Haskap.DddBase.Domain.UserAggregate.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (failedPermissions is not null && failedPermissions.Any(x => !string.IsNullOrWhiteSpace(x)))
            {
                throw new ForbiddenOperationException(string.Join(',', failedPermissions));
            }
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
