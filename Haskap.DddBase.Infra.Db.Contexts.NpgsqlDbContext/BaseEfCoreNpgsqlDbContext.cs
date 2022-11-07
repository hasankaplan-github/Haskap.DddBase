using Microsoft.EntityFrameworkCore;
using System;
using Haskap.DddBase.Utilities.ExtensionMethods;
using System.Linq;
using Haskap.DddBase.Domain;
using System.Threading.Tasks;
using System.Threading;

namespace Haskap.DddBase.Infra.Db.Contexts.NpgsqlDbContext;

public class BaseEfCoreNpgsqlDbContext : DbContext
{
    ///public DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }

    public BaseEfCoreNpgsqlDbContext(DbContextOptions<BaseEfCoreNpgsqlDbContext> options) : base(options)
    {
    }

    protected BaseEfCoreNpgsqlDbContext(DbContextOptions options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //foreach (var entityType in builder.Model.GetEntityTypes())
        //{
        //    entityType.SetTableName(entityType.DisplayName().ToSnakeCase(CaseOption.LowerCase));

        //    foreach (var property in entityType.GetProperties())
        //        property.SetColumnName(property.Name.ToSnakeCase(CaseOption.LowerCase));

        //    //foreach (var key in entityType.GetKeys())
        //    //    key.SetName(key.GetName().ToSnakeCase(CaseOption.LowerCase));

        //    //foreach (var foreignKey in entityType.GetForeignKeys())
        //    //    foreignKey.PrincipalKey.SetName(foreignKey.PrincipalKey.GetName().ToSnakeCase(CaseOption.LowerCase));
        //    ////foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase(CaseOption.LowerCase));

        //    //foreach (var index in entityType.GetIndexes())
        //    //    index.SetName(index.GetName().ToSnakeCase(CaseOption.LowerCase));
        //}
    }
}
