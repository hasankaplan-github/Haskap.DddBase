using Haskap.DddBase.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public class LocalDateTimeProvider : ILocalDateTimeProvider
{
    public string SystemTimeZoneId { get; set; }

    public DateTime ConvertToLocal(DateTime dateTime)
    {
        SystemTimeZoneId = SystemTimeZoneId == string.Empty ? "GMT Standard Time" : SystemTimeZoneId;
        return DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime.ToUniversalTime(), SystemTimeZoneId), DateTimeKind.Local);
    }
}
