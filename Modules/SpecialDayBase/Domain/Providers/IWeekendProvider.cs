namespace Modules.SpecialDayBase.Domain.Providers;

/// <summary>
/// WeekendProvider Interface
/// </summary>
public interface IWeekendProvider
{
    /// <summary>
    /// Get weekend days
    /// </summary>
    IEnumerable<DayOfWeek> WeekendDays { get; }

    /// <summary>
    /// Is given date in the weekend
    /// </summary>
    /// <param name="date"></param>
    /// <returns>True if given date is weekend, false otherwise</returns>
    bool IsWeekend(DateOnly date);

    /// <summary>
    /// Is given day in the weekend
    /// </summary>
    /// <param name="dayOfWeek"></param>
    /// <returns>True if given day of week is in the weekend, false otherwise</returns>
    bool IsWeekend(DayOfWeek dayOfWeek);

    /// <summary>
    /// Get first weekend day
    /// </summary>
    DayOfWeek FirstWeekendDay { get; }

    /// <summary>
    /// Get last weekend day
    /// </summary>
    DayOfWeek LastWeekendDay { get; }
}
