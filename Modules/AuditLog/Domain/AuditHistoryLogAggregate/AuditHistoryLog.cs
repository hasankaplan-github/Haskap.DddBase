using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Shared.Enums;

namespace Modules.AuditLog.Domain.AuditHistoryLogAggregate;

public class AuditHistoryLog : AggregateRoot, IHasMultiTenant
{
    public Guid? VisitId { get; set; }
    public Guid? TenantId { get; set; }
    public DateTime ModificationDateUtc { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public AuditHistoryLogModificationType ModificationType { get; set; }
    public string ObjectFullType { get; set; }
    public string? ObjectIds { get; set; } //value objectler için nullable
    public string? ObjectOriginalValues { get; set; } //json
    public string? ObjectNewValues { get; set; } //json
    public AuditHistoryLogOwnershipType OwnershipType { get; set; } = AuditHistoryLogOwnershipType.None;

    private AuditHistoryLog()
    {
    }

    public AuditHistoryLog(Guid id)
        : base(id)
    {

    }
}
