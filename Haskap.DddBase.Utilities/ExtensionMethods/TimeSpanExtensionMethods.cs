using System.Runtime.CompilerServices;

namespace Haskap.DddBase.Utilities.ExtensionMethods;

public static class TimeSpanExtensionMethods
{
    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan, CancellationToken cancellationToken = default)
    {
        return Task.Delay(timeSpan, cancellationToken).GetAwaiter();
    }

    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        return Task.Delay(timeSpan).GetAwaiter();
        // usage: await TimeSpan.FromSeconds(2);
    }
}
