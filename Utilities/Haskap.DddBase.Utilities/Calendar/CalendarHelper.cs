using System.Globalization;

namespace Haskap.DddBase.Utilities.Calendar;
public class CalendarHelper
{
    public static readonly Dictionary<(int HijriYear, int HijriMonth), int> HijriAdjsutmentsForTurkiye = new()
    {
        // Format: { (HijriYear, HijriMonth), Offset }
        { (1447, 10), -1 }, // 2026 March
        { (1447, 12), -1 }, // 2026 May
        { (1448, 10), 0 },
        { (1448, 12), 0 },
        { (1449, 10), 0 },
        { (1449, 12), -1 },  // 2028 May
        { (1450, 10), 0 },
        { (1450, 12), -1 },  // 2029 April
        { (1451, 10), 0 },
        { (1451, 12), 0 },
        { (1452, 10), 0 },
        { (1452, 12), 0 },
        { (1453, 10), -1 },   // 2032 January
        { (1453, 12), -1 },   // 2032 March
        { (1454, 10), 0 },
        { (1454, 12), 0 },
        { (1455, 10), -1 },   // 2033 December
        { (1455, 12), -1 },   // 2034 March
        { (1456, 10), -1 },   // 2034 December
        { (1456, 12), -1 },    // 2035 February
        { (1457, 10), 0 },
    };

    public static IEnumerable<int> GetHijriYears(int year)
    {
        //var diff = year - 621;
        //var hijriYear = Convert.ToInt32(Math.Round(diff + decimal.Divide(diff, 33), MidpointRounding.ToZero));
        //return [hijriYear, hijriYear + 1];


        var hijriCalendar = new HijriCalendar();

        var minHijriYear = hijriCalendar.GetYear(new DateTime(year, 1, 1));
        var maxHijriYear = hijriCalendar.GetYear(new DateTime(year, 12, 31));

        return Enumerable.Range(minHijriYear, maxHijriYear - minHijriYear + 1);
    }

    public static DateOnly? FindOccurrenceOfDayOfWeek(int year, int month, DayOfWeek day, int occurrenceInAMonth)
    {
        var firstDayOfMonth = new DateOnly(year, month, 1);

        //Substract first day of the month with the required day of the week
        var daysNeeded = (int)day - (int)firstDayOfMonth.DayOfWeek;

        //if it is less than zero we need to get the next week day (add 7 days)
        if (daysNeeded < 0)
        {
            daysNeeded += 7;
        }

        //multiply by the Occurance to get the day
        var resultedDay = (daysNeeded + 1) + (7 * (occurrenceInAMonth - 1));

        if (resultedDay > DateTime.DaysInMonth(year, month))
        {
            return null;
        }

        return new DateOnly(year, month, resultedDay);
    }

    public static DateOnly FindLastDayOfWeek(int year, int month, DayOfWeek day)
    {
        var lastDayOfMonth = new DateOnly(year, month, DateTime.DaysInMonth(year, month));

        var daysNeeded = (int)lastDayOfMonth.DayOfWeek - (int)day;

        if (daysNeeded < 0)
        {
            daysNeeded += 7;
        }
        var resultedDay = lastDayOfMonth.AddDays(-daysNeeded);
        return resultedDay;
    }

    public static int GetYearCountBetweenTwoDates(DateOnly startDate, DateOnly endDate)
    {
        return endDate.Year - startDate.Year + 1;
    }


    public static int GetMonthCountBetweenTwoDates(DateOnly startDate, DateOnly endDate)
    {
        return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;
    }
}
