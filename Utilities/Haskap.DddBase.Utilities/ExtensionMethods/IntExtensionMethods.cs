namespace Haskap.DddBase.Utilities.ExtensionMethods;

public static class IntExtensionMethods
{
    public static TimeSpan SecondsDelay(this int seconds)
    {
        return TimeSpan.FromSeconds(seconds);
        // usage: await 2.SecondsDelay();
    }
}
