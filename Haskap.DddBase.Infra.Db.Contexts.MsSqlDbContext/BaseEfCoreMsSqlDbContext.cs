using Microsoft.EntityFrameworkCore;

namespace Haskap.DddBase.Infrastructure.Data.EfCoreDbContexts.MsSqlDbContext
{
    public class BaseEfCoreMsSqlDbContext : DbContext
    {
        //public DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }

        public BaseEfCoreMsSqlDbContext(DbContextOptions<BaseEfCoreMsSqlDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreMsSqlDbContext(DbContextOptions options)
        : base(options)
        {
        }
    }
}
