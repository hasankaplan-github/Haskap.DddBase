using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Modules.Localization.Application.Contracts;
using Modules.Localization.Application.Dtos;
using Modules.Localization.Application.Mappings;
using Modules.Localization.Domain;
using Modules.Localization.Domain.SupportedLocaleAggregate;

namespace Modules.Localization.Application;

public class LocalizationService : UseCaseService, ILocalizationService
{
    private readonly ILocalizationDbContext _localizationDbContext;
    private readonly IMemoryCache _memoryCache;


    public LocalizationService(
        ILocalizationDbContext localizationDbContext,
        IMemoryCache memoryCache)
    {
        _localizationDbContext = localizationDbContext;
        _memoryCache = memoryCache;
    }

    public async Task InvalidateAllLocalesCachesAsync(CancellationToken cancellationToken = default)
    {
        var supportedLocales = await _localizationDbContext.SupportedLocale
            .AsNoTracking()
            .Select(x => x.Locale)
            .ToListAsync(cancellationToken);

        foreach (var locale in supportedLocales)
        {
            _memoryCache.Remove(locale);
        }
    }

    public async Task InvalidateLocaleCacheAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        _memoryCache.Remove(new Locale(localeValue));
    }

    public async Task AddLocalizationAsync(AddLocalizationInputDto input, CancellationToken cancellationToken = default)
    {
        var localization = await _localizationDbContext.Localization
            .Where(x => x.Locale.Value == input.LocaleValue && x.Key == input.Key)
            .FirstOrDefaultAsync(cancellationToken);

        if (localization is not null)
        {
            throw new InvalidOperationException($"Localization with key '{input.Key}' and locale '{input.LocaleValue}' already exists.");
        }

        localization = new Domain.LocalizationAggregate.Localization(GuidGenerator.CreateSimpleGuid(), input.Key, new Locale(input.LocaleValue), input.Value);
        await _localizationDbContext.Localization.AddAsync(localization, cancellationToken);
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLocalizationAsync(UpdateLocalizationInputDto input, CancellationToken cancellationToken = default)
    {
        var localization = await _localizationDbContext.Localization
            .Where(x => x.Id == input.LocalizationId)
            .FirstAsync(cancellationToken);

        localization.UpdateValue(input.NewValue);
        localization.UpdateLocale(new Locale(input.NewLocaleValue));

        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteLocalizationAsync(Guid localizationId, CancellationToken cancellationToken = default)
    {
        var localization = await _localizationDbContext.Localization
            .Where(x => x.Id == localizationId)
            .FirstAsync(cancellationToken);

        _localizationDbContext.Localization.Remove(localization);
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _localizationDbContext.Localization.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;

        if (input.Key is not null)
        {
            filtered = true;
            query = query.Where(x => x.Key.Contains(input.Key));
        }

        if (input.LocaleValue is not null)
        {
            filtered = true;
            query = query.Where(x => x.Locale.Value == input.LocaleValue);
        }

        if(input.Value is not null)
        {
            filtered = true;
            query = query.Where(x => x.Value.Contains(input.Value));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Key);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Key);
                }
            }
            else if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Locale.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Locale.Value);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Value);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Key);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var localizations = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var localizationOutputDtos = localizations.Select(x => x.ToLocalizationOutputDto()).ToList();

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = localizationOutputDtos
        };
    }


    public async Task<IList<SupportedLocaleOutputDto>> GetActiveSupportedLocalesAsync(CancellationToken cancellationToken = default)
    {
        return await _localizationDbContext.SupportedLocale
            .Where(x => x.IsActive)
            .Select(x => x.ToSupportedLocaleOutputDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<SupportedLocaleOutputDto>> GetAllSupportedLocalesAsync(CancellationToken cancellationToken = default)
    {
        return await _localizationDbContext.SupportedLocale
            .Select(x => x.ToSupportedLocaleOutputDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> ListSupportedLocalesAsync(JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var query = _localizationDbContext.SupportedLocale.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Locale.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Locale.Value);
                }
            }
            else if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.IsActive);
                }
                else
                {
                    query = query.OrderByDescending(x => x.IsActive);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.IsDefault);
                }
                else
                {
                    query = query.OrderByDescending(x => x.IsDefault);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Locale.Value);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var supportedLocales = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var supportedLocaleOutputDtos = supportedLocales.Select(x => x.ToSupportedLocaleOutputDto()).ToList();

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = supportedLocaleOutputDtos
        };
    }



    public async Task SetDefaultLocaleAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        var supportedLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.Locale.Value == localeValue)
            .FirstOrDefaultAsync(cancellationToken);
        if (supportedLocale is null)
        {
            throw new InvalidOperationException($"Locale '{localeValue}' is not supported.");
        }

        var currentDefaultLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.IsDefault)
            .FirstOrDefaultAsync(cancellationToken);
        if (currentDefaultLocale is not null && currentDefaultLocale.Id != supportedLocale.Id)
        {
            currentDefaultLocale.UnsetAsDefault();
        }

        supportedLocale.SetAsDefault();
        supportedLocale.Activate();

        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<LocaleOutputDto?> GetDefaultLocaleAsync(CancellationToken cancellationToken = default)
    {
        return await _localizationDbContext.SupportedLocale
            .Where(x => x.IsDefault)
            .Select(x => new LocaleOutputDto { Value = x.Locale.Value })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        var newLocale = new Locale(localeValue);

        var existingLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.Locale.Value == localeValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingLocale is not null)
        {
            throw new InvalidOperationException($"Locale '{localeValue}' is already supported and active.");
        }

        var supportedLocale = new SupportedLocale(GuidGenerator.CreateSimpleGuid(), newLocale);
        await _localizationDbContext.SupportedLocale.AddAsync(supportedLocale, cancellationToken);
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        var supportedLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.Locale.Value == localeValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (supportedLocale is null)
        {
            throw new InvalidOperationException($"Locale '{localeValue}' is not supported.");
        }

        if (supportedLocale.IsDefault)
        {
            throw new InvalidOperationException($"Cannot remove the default locale '{localeValue}'. Please set another locale as default before removing this one.");
        }

        _localizationDbContext.SupportedLocale.Remove(supportedLocale);
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActivateSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        var supportedLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.Locale.Value == localeValue)
            .FirstOrDefaultAsync(cancellationToken);
     
        if (supportedLocale is null)
        {
            throw new InvalidOperationException($"Locale '{localeValue}' is not supported.");
        }

        supportedLocale.Activate();
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateSupportedLocaleAsync(string localeValue, CancellationToken cancellationToken = default)
    {
        var supportedLocale = await _localizationDbContext.SupportedLocale
            .Where(x => x.Locale.Value == localeValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (supportedLocale is null)
        {
            throw new InvalidOperationException($"Locale '{localeValue}' is not supported.");
        }

        if (supportedLocale.IsDefault)
        {
            throw new InvalidOperationException($"Cannot deactivate the default locale '{localeValue}'. Please set another locale as default before deactivating this one.");
        }

        supportedLocale.Deactivate();
        await _localizationDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<SelectedLocaleOutputDto?> GetSelectedLocaleForUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var selectedLocale = await _localizationDbContext.SelectedLocale
            .Where(x => x.UserId == userId)
            .Select(x => new SelectedLocaleOutputDto
            {
                UserId = x.UserId,
                LocaleValue = x.Locale.Value
            })
            .FirstOrDefaultAsync(cancellationToken);

        return selectedLocale;
    }
}
