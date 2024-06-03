using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
using Haskap.DddBase.Domain.Providers;

namespace Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;

public class BaseEfCoreNpgsqlDbContext : BaseContext
{
    protected BaseEfCoreNpgsqlDbContext(
        DbContextOptions options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterGenericProvider? globalQueryFilterGenericProvider)
        : base(
            options, 
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
