using Microsoft.EntityFrameworkCore;
using System;
using Haskap.DddBase.Utilities.ExtensionMethods;
using System.Linq;
using Haskap.DddBase.Domain.Core;
using Haskap.DddBase.Infrastructure.Providers;
using System.Threading.Tasks;
using System.Threading;

namespace Haskap.DddBase.Infrastructure.Data.EfCoreDbContexts.NpgsqlDbContext
{
    public class BaseEfCoreNpgsqlDbContext<TUserId> : DbContext
    {
        private readonly LoggedInUserProvider<TUserId> loggedInUserProvider;
        public BaseEfCoreNpgsqlDbContext(DbContextOptions<BaseEfCoreNpgsqlDbContext<TUserId>> options, LoggedInUserProvider<TUserId> loggedInUserProvider) : base(options)
        {
            this.loggedInUserProvider = loggedInUserProvider;
        }

        protected BaseEfCoreNpgsqlDbContext(DbContextOptions options, LoggedInUserProvider<TUserId> loggedInUserProvider)
        : base(options)
        {
            this.loggedInUserProvider = loggedInUserProvider;
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

        private void SetAuditProperties()
        {
            var addedOrModifiedEntities = ChangeTracker.Entries()
                                        .Where(x => x.Entity is IAuditable<TUserId> && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in addedOrModifiedEntities)
            {
                var e = entity.Entity as IAuditable<TUserId>;

                if (entity.State == EntityState.Added)
                {
                    e.CreatedAt = DateTime.UtcNow;
                    e.CreatedUserId = loggedInUserProvider.UserId;
                }
                else if (entity.State == EntityState.Modified)
                {
                    e.ModifiedAt = DateTime.UtcNow;
                    e.ModifiedUserId = loggedInUserProvider.UserId;
                }
            }

        }

        public override int SaveChanges()
        {
            SetAuditProperties();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetAuditProperties();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested == false)
            {
                SetAuditProperties();
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested == false)
            {
                SetAuditProperties();
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
