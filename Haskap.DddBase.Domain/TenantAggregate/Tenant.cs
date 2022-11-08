using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.TenantAggregate;

public class Tenant : AggregateRoot<Guid>
{
    public static Tenant Empty = new(Guid.Empty, "EmptyTenant");
    public static Tenant Admin = new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "AdminTenant");    

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
