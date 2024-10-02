using Haskap.DddBase.Domain;
using Haskap.DddBase.Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Domain;
public interface IViewLevelExceptionsDbContext : IUnitOfWork
{
    DbSet<ViewLevelException> ViewLevelException { get; set; }
}
