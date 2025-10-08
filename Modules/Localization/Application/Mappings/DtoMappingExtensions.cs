using Haskap.DddBase.Application.Mappings;
using Modules.Localization.Application.Dtos;
using Modules.Localization.Domain.SupportedLocaleAggregate;

namespace Modules.Localization.Application.Mappings;
public static class DtoMappingExtensions
{
    public static LocalizationOutputDto ToLocalizationOutputDto(this Domain.LocalizationAggregate.Localization localization)
    {
        return new()
        {
            Id = localization.Id,
            Key = localization.Key,
            LocaleValue = localization.Locale.Value,
            Value = localization.Value
        };
    }

    public static SupportedLocaleOutputDto ToSupportedLocaleOutputDto(this SupportedLocale supportedLocale)
    {
        return new()
        {
            Id = supportedLocale.Id,
            Locale = supportedLocale.Locale.ToLocaleOutputDto(),
            IsActive = supportedLocale.IsActive,
            IsDefault = supportedLocale.IsDefault,
        };
    }
}
