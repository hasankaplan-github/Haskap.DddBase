using Haskap.DddBase.Application.Dtos.Common;

namespace Modules.Localization.Application.Dtos;
public class SupportedLocaleOutputDto
{
    public Guid Id { get; set; }
    public LocaleOutputDto Locale { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}
