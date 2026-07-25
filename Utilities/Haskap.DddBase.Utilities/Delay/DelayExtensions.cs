using System.Runtime.CompilerServices;

namespace Haskap.DddBase.Utilities.Delay;

public static class DelayExtensions
{
    public static TimeSpan SecondsDelay(this int seconds)
    {
        return TimeSpan.FromSeconds(seconds);
        // usage: await 2.SecondsDelay();
    }

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
