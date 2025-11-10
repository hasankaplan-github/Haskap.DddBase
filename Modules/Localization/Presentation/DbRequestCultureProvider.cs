using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Modules.Localization.Application.Contracts;

namespace Modules.Localization.Presentation;
public class DbRequestCultureProvider : RequestCultureProvider
{
    public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (!httpContext.User.TryFindUserId(out Guid userId))
        {
            return await NullProviderCultureResult;
        }

        var localizationService = httpContext.RequestServices.GetRequiredService<ILocalizationService>();
        var memoryCache = httpContext.RequestServices.GetRequiredService<IMemoryCache>();
        var baseCacheKeyProvider = httpContext.RequestServices.GetRequiredService<IBaseCacheKeyProvider>();

        var selectedLocale = await memoryCache.GetOrCreateAsync(baseCacheKeyProvider.GetSelectedCultureCacheKey(userId), async (cacheEntry) =>
        {
            var userCts = memoryCache.Get<CancellationTokenSource>(baseCacheKeyProvider.GetUserCancellationTokenSourceCacheKey(userId));
            cacheEntry.AddExpirationToken(new CancellationChangeToken(userCts!.Token));
            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(10);

            return await localizationService.GetSelectedLocaleForUser(userId);
        });

        return selectedLocale is null
            ? await NullProviderCultureResult
            : new ProviderCultureResult(selectedLocale.LocaleValue, selectedLocale.LocaleValue);
    }
}
