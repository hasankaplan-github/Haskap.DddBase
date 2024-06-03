using Haskap.DddBase.Application.Dtos.ViewLevelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.Contracts.ViewLevelExceptions;
public interface IViewLevelExceptionService : IUseCaseService
{
    Task<ViewLevelExceptionOutputDto> GetViewLevelExceptionAsync(Guid id);
    Task DeleteViewLevelExceptionAsync(Guid id);
    Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto inputDto);
}
