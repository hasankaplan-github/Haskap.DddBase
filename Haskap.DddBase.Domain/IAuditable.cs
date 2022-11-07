using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain
{
    public interface IAuditable<TUserId>
    {
        TUserId CreatedUserId { get; set; }
        DateTime? CreatedAt { get; set; }
        TUserId ModifiedUserId { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}
