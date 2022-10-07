using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
