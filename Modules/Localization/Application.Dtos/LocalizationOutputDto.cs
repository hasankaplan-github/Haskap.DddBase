using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Localization.Application.Dtos;
public class LocalizationOutputDto
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public LocaleOutputDto Locale { get; set; }
    public string Value { get; set; }
}
