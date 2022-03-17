using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core.AuditHistoryLogAggregate;

public enum AuditHistoryLogModificationType
{
    Add,
    Update,
    Delete,
    SoftDelete,
    Undelete
}
