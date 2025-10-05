using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Localization.Application.Dtos;
public class SelectedLocaleOutputDto
{
    public string LocaleValue { get; set; }
    public Guid UserId { get; set; }
}
