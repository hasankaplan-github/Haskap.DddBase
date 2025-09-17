using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Modules.Localization.Domain;
using System.Security.Claims;

namespace Modules.Localization.Presentation;
public class DbRequestCultureProvider : RequestCultureProvider
{
    public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (!Guid.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            return await NullProviderCultureResult;
        }

        var localizationDbContext = (httpContext.RequestServices.GetService(typeof(ILocalizationDbContext)) as ILocalizationDbContext)!;
        var memoryCache = (httpContext.RequestServices.GetService(typeof(IMemoryCache)) as IMemoryCache)!;
        var baseCacheKeyProvider = (httpContext.RequestServices.GetService(typeof(IBaseCacheKeyProvider)) as IBaseCacheKeyProvider)!;

        var selectedCulture = await memoryCache.GetOrCreateAsync(baseCacheKeyProvider.GetSelectedCultureCacheKey(userId), async (cacheEntry) =>
        {
            var userCts = memoryCache.Get<CancellationTokenSource>(baseCacheKeyProvider.GetUserCancellationTokenSourceCacheKey(userId));
            cacheEntry.AddExpirationToken(new CancellationChangeToken(userCts!.Token));
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);
            
            return await localizationDbContext.SelectedCulture
                .Where(x => x.UserId == userId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();
        });

        return selectedCulture is null
            ? await NullProviderCultureResult
            : new ProviderCultureResult(selectedCulture, selectedCulture);
    }
}
