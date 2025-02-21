using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.AuditLog.Domain.AuditHistoryLogAggregate;
using Modules.ModuleManagement.Domain.ModuleAggregate;

namespace Modules.ModuleManagement.Domain;
public interface IModuleManagementDbContext : IUnitOfWork
{
    DbSet<EnabledModule> EnabledModule { get; set; }
    DbSet<AuditHistoryLog> AuditHistoryLog { get; set; }
}
