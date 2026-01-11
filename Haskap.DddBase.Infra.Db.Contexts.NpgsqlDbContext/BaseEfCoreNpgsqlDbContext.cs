using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
using Microsoft.EntityFrameworkCore;

namespace Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;

public class BaseEfCoreNpgsqlDbContext : BaseContext
{
    protected BaseEfCoreNpgsqlDbContext(
        DbContextOptions options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(
            options, 
            currentTenantProvider,
            globalQueryFilterManagerProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
