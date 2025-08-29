using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.ModuleManagement.Domain.ModuleAggregate;

namespace Modules.ModuleManagement.Domain;
public interface IModuleManagementDbContext : IUnitOfWork
{
    DbSet<EnabledModule> EnabledModule { get; set; }
}
