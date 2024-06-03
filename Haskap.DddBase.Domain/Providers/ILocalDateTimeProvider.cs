using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;
public interface ILocalDateTimeProvider
{
    string SystemTimeZoneId { get; set; }
    DateTime ConvertToLocal(DateTime dateTime);
}
