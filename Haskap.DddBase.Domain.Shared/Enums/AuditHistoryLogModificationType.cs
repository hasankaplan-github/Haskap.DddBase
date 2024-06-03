using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Shared.Enums;

public enum AuditHistoryLogModificationType
{
    None,
    Add,
    Update,
    Delete,
    SoftDelete,
    Undelete
}
