using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core.TenantAggregate;

public class Tenant : AggregateRoot<Guid>
{
    public string Code { get; private set; }

    private Tenant()
    {
    }
    
    public Tenant(Guid id, string code)
        : base(id)
    {
        Code = Guard.Against.NullOrWhiteSpace(code);
    }
}
