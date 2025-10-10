using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Localization.Application.Dtos;
public class UpdateLocalizationInputDto
{
    public Guid LocalizationId { get; set; }
    public string NewValue { get; set; }
    public string NewKey { get; set; }
}
