using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;

public interface IHasMultiTenant
{
    Guid? TenantId { get; set; }
}
