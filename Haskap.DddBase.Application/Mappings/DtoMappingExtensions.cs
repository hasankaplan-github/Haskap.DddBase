using Haskap.DddBase.Application.Dtos.Common;
using Haskap.DddBase.Domain.Common;

namespace Haskap.DddBase.Application.Mappings;
public static class DtoMappingExtensions
{
    public static FileOutputDto ToFileOutputDto(this Haskap.DddBase.Domain.Common.File file)
    {
        return new()
        {
            Id = file.Id,
            OriginalName = file.OriginalName,
            NewName = file.NewName,
            Extension = file.Extension
        };
    }

    public static PermissionOutputDto ToPermissionOutputDto(this Permission permission)
    {
        return new()
        {
            Name = permission.Name
        };
    }

    public static LocaleOutputDto ToLocaleOutputDto(this Locale locale)
    {
        return new()
        {
            Value = locale.Value,
            FlagIconCssClass = locale.FlagIconCssClass,
        };
    }
}
