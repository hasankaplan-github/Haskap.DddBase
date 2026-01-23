using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Domain;

public interface IBpmDbContext : IUnitOfWork
{
    DbSet<Process> Process { get; set; }
    DbSet<ProcessAggregate.Path> Path { get; set; }
    DbSet<Progress> Progress { get; set; }
    DbSet<Request> Request { get; set; }
    DbSet<State> State { get; set; }
    DbSet<Command> Command { get; set; }
}
