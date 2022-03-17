using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core.Attributes.AuditHistoryLogAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RemoveAuditHistoryLog : Attribute
    {
    }
}
