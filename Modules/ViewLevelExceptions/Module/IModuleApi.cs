using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Module;
public interface IModuleApi
{
    Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto inputDto);
}
