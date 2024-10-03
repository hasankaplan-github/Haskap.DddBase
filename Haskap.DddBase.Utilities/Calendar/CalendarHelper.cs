using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities.Calendar;
public class CalendarHelper
{
    public IEnumerable<int> GetHijriYears(int year)
    {
        //var diff = year - 621;
        //var hijriYear = Convert.ToInt32(Math.Round(diff + decimal.Divide(diff, 33), MidpointRounding.ToZero));
        //return [hijriYear, hijriYear + 1];


        var hijriCalendar = new UmAlQuraCalendar();

        var minHijriYear = hijriCalendar.GetYear(new DateTime(year, 1, 1));
        var maxHijriYear = hijriCalendar.GetYear(new DateTime(year, 12, 31));

        return Enumerable.Range(minHijriYear, maxHijriYear - minHijriYear + 1);
    }

    public DateOnly FindOccurrenceOfDayOfWeek(int year, int month, DayOfWeek day, SpecialDayOccurrenceInAMonth occurrence)
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
        var resultedDay = (daysNeeded + 1) + (7 * ((int)occurrence - 1));

        if (resultedDay > DateTime.DaysInMonth(year, month))
        {
            throw new InvalidOperationException();
        }

        return new DateOnly(year, month, resultedDay);
    }
}
