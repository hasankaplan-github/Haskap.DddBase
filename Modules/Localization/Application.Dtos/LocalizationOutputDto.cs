using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Localization.Application.Dtos;
public class LocalizationOutputDto
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string LocaleValue { get; set; }
    public string Value { get; set; }
}
