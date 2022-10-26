using Haskap.DddBase.Utilities.Guids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;

public class VisitIdProvider
{
    public Guid? VisitId { get; set; } = null;

    public void Generate()
    {
        VisitId = GuidGenerator.CreateSimpleGuid();
    }
}
