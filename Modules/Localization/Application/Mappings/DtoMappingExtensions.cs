using Modules.Localization.Application.Dtos;

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
}
