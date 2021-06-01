using Microsoft.EntityFrameworkCore;
using System;

namespace Haskap.DddBase.Infrastructure.DataAccess.EfCoreDbContexts.MsSqlDbContext
{
    public class BaseEfCoreMsSqlDbContext : DbContext
    {
        public BaseEfCoreMsSqlDbContext(DbContextOptions<BaseEfCoreMsSqlDbContext> options) : base(options)
        {
        }

        protected BaseEfCoreMsSqlDbContext(DbContextOptions options)
        : base(options)
        {
        }
    }
}
