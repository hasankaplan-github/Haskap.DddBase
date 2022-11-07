using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.Core.TenantAggregate;

public class Tenant : AggregateRoot<Guid>
{
    public static Tenant EmptyTenant = new (Guid.Empty, "EmptyTenant");

    public const string ClaimKey = "tenantKey";
    public const string HeaderKey = "tenantKey";
    public const string CookieKey = "tenantKey";

    public string Name { get; private set; }

    private Tenant()
    {
    }
    
    public Tenant(Guid id, string name)
        : base(id)
    {
        Name = name;
    }
}
