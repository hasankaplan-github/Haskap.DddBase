using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;

public interface IAuditable
{
    Guid? CreatedUserId { get; set; }
    DateTime? CreatedAt { get; set; }
    Guid? ModifiedUserId { get; set; }
    DateTime? ModifiedAt { get; set; }
}
