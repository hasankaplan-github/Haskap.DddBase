using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ModuleManagement.Application.Dtos.Module;
public class IsEnabledInputDto
{
    public Guid? TenantId { get; set; }
    public string ModuleName { get; set; }
}
