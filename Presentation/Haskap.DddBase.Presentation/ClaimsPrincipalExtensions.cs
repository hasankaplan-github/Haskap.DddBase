using Haskap.DddBase.Domain.Shared.Consts;
using System.Security.Claims;

namespace Haskap.DddBase.Presentation;
public static class ClaimsPrincipalExtensions
{
    public static bool TryFindUserId(this ClaimsPrincipal? user, out Guid userId)
    {
        return Guid.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userId);
    }

    public static bool TryFindLoginId(this ClaimsPrincipal? user, out Guid loginId)
    {
        return Guid.TryParse(user?.FindFirst(AccountConsts.LoginIdClaimType)?.Value, out loginId);
    }

    public static bool TryFindTenantId(this ClaimsPrincipal? user, out Guid? tenantId)
    {
        if (user is null)
        {
            tenantId = null;
            return false;
        }

        if (Guid.TryParse(user.FindFirst(TenantConsts.IdClaimType)?.Value, out Guid tempTenantId))
        {
            tenantId = tempTenantId;
            return true;
        }

        tenantId = null;
        return true;
    }
}
