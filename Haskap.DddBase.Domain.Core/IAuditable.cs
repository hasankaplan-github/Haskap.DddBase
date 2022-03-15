using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core
{
    public interface IAuditable<TId>
    {
        TId CreatedUserId { get; set; }
        DateTime? CreatedAt { get; set; }
        TId ModifiedUserId { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}
