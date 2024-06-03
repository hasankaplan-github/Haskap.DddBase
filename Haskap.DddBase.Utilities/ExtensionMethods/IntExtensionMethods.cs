using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities.ExtensionMethods;

public static class IntExtensionMethods
{
    public static TimeSpan SecondsDelay(this int seconds)
    {
        return TimeSpan.FromSeconds(seconds);
        // usage: await 2.SecondsDelay();
    }
}
