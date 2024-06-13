using Haskap.DddBase.Domain.TenantAggregate;
using Haskap.DddBase.Domain.ViewLevelExceptionAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;
public interface IBaseDbContext : IUnitOfWork
{
    DbSet<Tenant> Tenant { get; set; }
    DbSet<ViewLevelException> ViewLevelException { get; set; }
}
