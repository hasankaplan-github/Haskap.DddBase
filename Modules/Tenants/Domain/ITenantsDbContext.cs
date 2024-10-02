using Haskap.DddBase.Domain;
using Haskap.DddBase.Modules.Tenants.Domain.TenantAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.Tenants.Domain;
public interface ITenantsDbContext : IUnitOfWork
{
    DbSet<Tenant> Tenant { get; set; }
}
