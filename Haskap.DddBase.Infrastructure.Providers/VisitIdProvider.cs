using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infrastructure.Providers
{
    public class VisitIdProvider
    {
        public Guid VisitId { get; private set; } = Guid.NewGuid();
    }
}
