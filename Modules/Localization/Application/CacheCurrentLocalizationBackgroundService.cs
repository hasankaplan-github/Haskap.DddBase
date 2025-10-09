using Haskap.DddBase.Domain.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modules.Localization.Application.Contracts;
using System.Globalization;
using System.Threading.Channels;

namespace Modules.Localization.Application;
public class CacheCurrentLocalizationBackgroundService : BackgroundService
{
    private readonly Lock _cachingLock = new();
    private readonly Channel<Locale> _currentLocaleChannel;
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CacheCurrentLocalizationBackgroundService> _logger;

    public CacheCurrentLocalizationBackgroundService(
        Channel<Locale> currentLocaleChannel,
        IMemoryCache memoryCache,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<CacheCurrentLocalizationBackgroundService> logger)
    {
        _currentLocaleChannel = currentLocaleChannel;
        _memoryCache = memoryCache;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentLocale = await _currentLocaleChannel.Reader.ReadAsync(stoppingToken);

                lock (_cachingLock)
                {
                    var isCached = _memoryCache.TryGetValue(currentLocale, out _);

                    if (isCached) continue;
                    CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(currentLocale.Value);
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbStringLocalizer = scope.ServiceProvider.GetRequiredService<IDbStringLocalizer>();
                    var localizedStrings = dbStringLocalizer.GetAllStrings(true).ToDictionary(x => x.Name);

                    _memoryCache.Set(currentLocale, localizedStrings, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromHours(24),
                    });

                    _logger.LogInformation("Cached localized strings for locale {Locale}.", currentLocale.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while caching localized strings.");
            }
        }
    }
}
