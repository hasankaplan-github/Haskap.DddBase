namespace Haskap.DddBase.Domain.Providers;

public interface IVisitIdProvider
{
    Guid? VisitId { get; set; }
}
