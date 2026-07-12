using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Haskap.DddBase.Utilities.ExtensionMethods;

public static class EnumExtensionMethods
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()?
                        .GetMember(enumValue.ToString())?
                        .First()?
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? string.Empty;
    }
}
