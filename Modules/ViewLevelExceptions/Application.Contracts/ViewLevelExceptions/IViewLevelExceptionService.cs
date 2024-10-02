using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
public interface IViewLevelExceptionService : IUseCaseService
{
    Task<ViewLevelExceptionOutputDto> GetViewLevelExceptionAsync(Guid id);
    Task DeleteViewLevelExceptionAsync(Guid id);
    Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto input);
}
