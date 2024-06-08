using Haskap.DddBase.Application.Dtos.Accounts;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Haskap.DddBase.Application.Contracts.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Haskap.DddBase.Domain.UserAggregate.Exceptions;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            context.Fail();
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        var permissions = await accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId });

        if (!permissions.Contains(requirement.Name))
        {
            context.Fail(new AuthorizationFailureReason(this, requirement.DisplayText ?? requirement.Name));
            return;
        }
        
        context.Succeed(requirement);
    }
}
