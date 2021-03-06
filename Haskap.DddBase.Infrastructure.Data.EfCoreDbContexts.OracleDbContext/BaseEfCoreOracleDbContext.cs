using Microsoft.EntityFrameworkCore;
using System;
using Haskap.DddBase.Utilities.ExtensionMethods;
using Haskap.DddBase.Domain.Core;

namespace Haskap.DddBase.Infrastructure.Data.EfCoreDbContexts.OracleDbContext
{
    public class BaseEfCoreOracleDbContext : DbContext
    {
        //public DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }

        public BaseEfCoreOracleDbContext(DbContextOptions<BaseEfCoreOracleDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreOracleDbContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    entityType.SetTableName(entityType.DisplayName().ToSnakeCase(CaseOption.UpperCase));

            //    foreach (var property in entityType.GetProperties())
            //        property.SetColumnName(property.Name.ToSnakeCase(CaseOption.UpperCase));

            //    //foreach (var key in entityType.GetKeys())
            //    //    key.SetName(key.GetName().ToSnakeCase(CaseOption.UpperCase));

            //    //foreach (var foreignKey in entityType.GetForeignKeys())
            //    //    foreignKey.PrincipalKey.SetName(foreignKey.PrincipalKey.GetName().ToSnakeCase(CaseOption.UpperCase));
            //    //foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase(CaseOption.UpperCase));

            //    //foreach (var index in entityType.GetIndexes())
            //    //    index.SetName(index.GetName().ToSnakeCase(CaseOption.UpperCase));
            //}
        }
    }
}
