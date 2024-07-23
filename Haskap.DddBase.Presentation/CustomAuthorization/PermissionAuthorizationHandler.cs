using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Claims;

namespace Haskap.DddBase.Presentation.CustomAuthorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Type _accountServiceType;

    public PermissionAuthorizationHandler(
        IServiceScopeFactory serviceScopeFactory,
        Type accountServiceType)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _accountServiceType = accountServiceType;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            context.Fail();
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var accountService = scope.ServiceProvider.GetRequiredService(_accountServiceType);

        MethodInfo getAllPermissionsAsyncMethodInfo = _accountServiceType.GetMethod("GetAllPermissionsAsync")!;
        ParameterInfo[] parameters = getAllPermissionsAsyncMethodInfo.GetParameters();
        object getAllPermissionsInputDtoParameterInstance = Activator.CreateInstance(parameters[0].ParameterType)!;
        PropertyInfo userIdPRopertyInfo = getAllPermissionsInputDtoParameterInstance.GetType().GetProperty("UserId")!;
        userIdPRopertyInfo.SetValue(getAllPermissionsInputDtoParameterInstance, userId);
        object[] parametersArray = [getAllPermissionsInputDtoParameterInstance, default(CancellationToken)];
        var permissionsTask = getAllPermissionsAsyncMethodInfo.Invoke(accountService, parametersArray) as Task<HashSet<string>>;

        var permissions = await permissionsTask!.ConfigureAwait(false);

        //var permissions = await accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId });

        if (!permissions.Contains(requirement.Name))
        {
            context.Fail(new AuthorizationFailureReason(this, requirement.DisplayText ?? requirement.Name));
            return;
        }
        
        context.Succeed(requirement);
    }
}
