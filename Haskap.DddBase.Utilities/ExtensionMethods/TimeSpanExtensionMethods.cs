using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities.ExtensionMethods;

public static class TimeSpanExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        return Task.Delay(timeSpan).GetAwaiter();
        // usage: await TimeSpan.FromSeconds(2);
    }
}
