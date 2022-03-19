using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core.AuditHistoryLogAggregate;

public class AuditHistoryLog<TUserId> : AggregateRoot<Guid>
{
    public Guid VisitId { get; set; }
    public DateTime ModificationDate { get; set; }
    public TUserId ModifiedUserId { get; set; }
    public AuditHistoryLogModificationType ModificationType { get; set; }
    public string ObjectFullType { get; set; }
    public string? ObjectIds { get; set; } //value objectler için nullable
    public string? ObjectOriginalValues { get; set; } //json
    public string? ObjectNewValues { get; set; } //json

    public AuditHistoryLog(Guid id)
        : base(id)
    {

    }
}
