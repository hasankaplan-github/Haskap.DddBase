using Haskap.DddBase.Domain.Common;

namespace Haskap.DddBase.Domain.Providers;
public interface IBaseCacheKeyProvider
{
    string GetAllPermissionsCacheKey(Guid userId);
    string GetUserPermissionsCacheKey(Guid userId);
    string GetRolePermissionsCacheKey(Guid roleId);
    string GetModuleStatusesCacheKey(Guid? tenantId);
    string GetUserCancellationTokenSourceCacheKey(Guid userId);
    string GetTenantCancellationTokenSourceCacheKey(Guid? tenantId);
    string GetSelectedCultureCacheKey(Guid userId);
    string GetLocalizationCacheKey(Guid? tenantId, Locale locale);
}
