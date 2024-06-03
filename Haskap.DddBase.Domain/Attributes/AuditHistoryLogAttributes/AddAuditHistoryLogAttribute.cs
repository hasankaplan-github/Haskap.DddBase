using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class AddAuditHistoryLogAttribute : Attribute
    {
    }
}
