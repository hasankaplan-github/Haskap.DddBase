using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;

namespace Haskap.DddBase.Infra.Providers;
public class VisitIdProvider : IVisitIdProvider
{
    public Guid? VisitId { get; set; } = null;

    public VisitIdProvider()
    {
        VisitId = GuidGenerator.CreateSimpleGuid();
    }
}
