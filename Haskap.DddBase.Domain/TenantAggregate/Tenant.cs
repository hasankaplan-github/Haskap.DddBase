using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.TenantAggregate;

public class Tenant : AggregateRoot<Guid>, IAuditable
{
    public static Tenant Empty = new(Guid.Empty, "EmptyTenant");
    public static Tenant Admin = new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "AdminTenant");    

    public const string ClaimKey = "tenantKey";
    public const string HeaderKey = "tenantKey";
    public const string CookieKey = "tenantKey";

    public string Name { get; private set; }


    public Guid? CreatedUserId { get; set; } = null;
    public DateTime? CreatedAt { get; set; } = null;
    public Guid? ModifiedUserId { get; set; } = null;
    public DateTime? ModifiedAt { get; set; } = null;

    private Tenant()
    {
    }
    
    public Tenant(Guid id, string name)
        : base(id)
    {
        Name = name;
    }
}
