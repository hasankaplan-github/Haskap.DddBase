using Haskap.DddBase.Domain;
using Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ViewLevelExceptions.Domain;
public interface IViewLevelExceptionsDbContext : IUnitOfWork
{
    DbSet<ViewLevelException> ViewLevelException { get; set; }
}
