using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Localization.Application.Dtos;
public class LocalizationCacheInfoDto
{
    public LocaleOutputDto Locale { get; set; }
    public string CacheKey { get; set; }
}
