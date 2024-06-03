using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.TenantAggregate.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain.TenantAggregate;
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
