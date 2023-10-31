using Haskap.DddBase.Utilities.Guids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Providers;

public interface IVisitIdProvider
{
    Guid? VisitId { get; set; }

    void Generate();
}
