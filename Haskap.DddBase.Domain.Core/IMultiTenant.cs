using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core;

public interface IMultiTenant
{
    Guid? TenantId { get; set; }
}
