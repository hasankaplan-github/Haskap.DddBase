using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Haskap.DddBase.Utilities.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string ToSnakeCase(this string input, CaseOption caseOption)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            var startUnderscores = Regex.Match(input, @"^_+");
            CultureInfo cultureInfo = new CultureInfo("en-US");
            var replacedText = Regex.Replace(input, @"([a-zA-Z0-9])([A-Z])", "$1_$2");
            replacedText = Regex.Replace(replacedText, @"([a-zA-Z])([0-9A-Z])", "$1_$2");
            replacedText = Regex.Replace(replacedText, @"([A-Z])([0-9])", "$1_$2");
            if (caseOption == CaseOption.LowerCase)
            {
                replacedText = replacedText.ToLower(cultureInfo);
            }
            else if (caseOption == CaseOption.UpperCase)
            {
                replacedText = replacedText.ToUpper(cultureInfo);
            }

            return startUnderscores + replacedText;
        }
    }
}
