using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.TenantAggregate;

public class Tenant : AggregateRoot<Guid>
{
    public static Tenant EmptyTenant = new(Guid.Empty, "EmptyTenant");
    public static Tenant AdminTenant = new(Guid.Parse("{9B55EE45-D859-458B-9D9C-DE8570969366}"), "AdminTenant");

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
