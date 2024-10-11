﻿using Haskap.DddBase.Domain;
using Modules.Tenants.Domain.TenantAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Tenants.Domain;
public interface ITenantsDbContext : IUnitOfWork
{
    DbSet<Tenant> Tenant { get; set; }
}
