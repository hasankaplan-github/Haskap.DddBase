using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Shared.Consts;
public class Modules
{
    public const string SectionName = "Modules";
    public Dictionary<string, bool> IsEnabled { get; set; }
}
