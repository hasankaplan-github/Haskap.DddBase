using Haskap.DddBase.Application.Mappings;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Http;
using Modules.Localization.Application.Dtos;
using Modules.Localization.Domain.Shared;
using System.Threading.Channels;

namespace Modules.Localization.Presentation.Middlewares;

public class WriteCurrentLocalizationCacheKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Channel<LocalizationCacheInfoDto> _localizationCacheInfoChannel;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;

    public WriteCurrentLocalizationCacheKeyMiddleware(
        RequestDelegate next,
        Channel<LocalizationCacheInfoDto> localizationCacheInfoChannel,
        IBaseCacheKeyProvider baseCacheKeyProvider)
    {
        _next = next;
        _localizationCacheInfoChannel = localizationCacheInfoChannel;
        _baseCacheKeyProvider = baseCacheKeyProvider;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ILocalizationModule localizationModule)
    {
        var currentTenantId = httpContext.FindTenantId();

        if (!await localizationModule.IsEnabledAsync(currentTenantId))
        {
            if (Locale.Default is not null)
            {
                var cacheKey = _baseCacheKeyProvider.GetLocalizationCacheKey(currentTenantId, Locale.Default);
                var localizationCacheInfo = new LocalizationCacheInfoDto
                {
                    CacheKey = cacheKey,
                    Locale = Locale.Default.ToLocaleOutputDto()
                };
                await _localizationCacheInfoChannel.Writer.WriteAsync(localizationCacheInfo);
            }
        }
        else
        {
            var cacheKey = _baseCacheKeyProvider.GetLocalizationCacheKey(currentTenantId, Locale.CurrentUiLocale);
            var localizationCacheInfo = new LocalizationCacheInfoDto
            {
                CacheKey = cacheKey,
                Locale = Locale.CurrentUiLocale.ToLocaleOutputDto()
            };
            await _localizationCacheInfoChannel.Writer.WriteAsync(localizationCacheInfo);
        }

        await _next(httpContext);
    }
}
