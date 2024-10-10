using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;

namespace Modules.AuditLog.Domain;
public interface IAuditLogDbContext : IUnitOfWork
{
    DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }
}
