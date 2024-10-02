using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Module;
internal class ModuleApi : IModuleApi
{
    private readonly IViewLevelExceptionService _viewLevelExceptionService;

    public ModuleApi(IViewLevelExceptionService viewLevelExceptionService)
    {
        _viewLevelExceptionService = viewLevelExceptionService;
    }

    public async Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto input)
    {
        return await _viewLevelExceptionService.SaveAndGetIdAsync(input);
    }
}
