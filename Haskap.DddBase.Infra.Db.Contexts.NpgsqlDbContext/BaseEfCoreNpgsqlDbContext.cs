using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
using Microsoft.EntityFrameworkCore;

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
