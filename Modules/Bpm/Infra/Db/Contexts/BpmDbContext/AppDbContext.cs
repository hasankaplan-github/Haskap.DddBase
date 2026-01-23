using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;
using Microsoft.EntityFrameworkCore;
using Modules.Bpm.Domain;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Infra.Db.Contexts.BpmDbContext;
public class AppDbContext : BaseEfCoreNpgsqlDbContext, IBpmDbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterGenericProvider)
        : base(
            options,
            currentTenantProvider,
            globalQueryFilterGenericProvider)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("bpm");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly, type => type.Namespace!.Contains("BpmDbContext"));

        base.OnModelCreating(builder);
    }

    public DbSet<Process> Process { get; set; }
    public DbSet<Domain.ProcessAggregate.Path> Path { get; set; }
    public DbSet<Progress> Progress { get; set; }
    public DbSet<Request> Request { get; set; }
    public DbSet<State> State { get; set; }
    public DbSet<Command> Command { get; set; }
}
