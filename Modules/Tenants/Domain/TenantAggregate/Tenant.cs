using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Tenants.Domain.TenantAggregate.Exceptions;

namespace Modules.Tenants.Domain.TenantAggregate;
public class Tenant : AggregateRoot<Guid>, ISoftDeletable
{
    public string Name { get; private set; }
    public bool IsDeleted { get; set; }

    private Tenant()
    {
    }

    public Tenant(Guid id, string name, DbSet<Tenant> tenantDbSet)
        : base(id)
    {
        SetName(name, tenantDbSet);
    }

    public void SetName(string name, DbSet<Tenant> tenantDbSet)
    {
        Guard.Against.NullOrWhiteSpace(name);
        
        var exist = tenantDbSet.Where(x => x.Name.ToLower().Equals(name.ToLower()))
            .Any();

        if (exist)
        {
            throw new DuplicateTenantNameException();
        }

        Name = name;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}
