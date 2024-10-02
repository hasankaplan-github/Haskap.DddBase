using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using Haskap.DddBase.Modules.ViewLevelExceptions.Domain;
using Haskap.DddBase.Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Application.UseCaseServices.ViewLevelExceptions;
public class ViewLevelExceptionService : UseCaseService, IViewLevelExceptionService
{
    private readonly IViewLevelExceptionsDbContext _viewLevelExceptionsDbContext;
    private readonly IMapper _mapper;

    public ViewLevelExceptionService(
        IViewLevelExceptionsDbContext viewLevelExceptionsDbContext,
        IMapper mapper)
    {
        _viewLevelExceptionsDbContext = viewLevelExceptionsDbContext;
        _mapper = mapper;
    }

    

    public async Task DeleteViewLevelExceptionAsync(Guid id)
    {
        await _viewLevelExceptionsDbContext.ViewLevelException
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<ViewLevelExceptionOutputDto> GetViewLevelExceptionAsync(Guid id)
    {
        var exception = await _viewLevelExceptionsDbContext.ViewLevelException
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        var output = _mapper.Map<ViewLevelExceptionOutputDto>(exception);

        return output;
    }

    public async Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto inputDto)
    {
        var viewLevelException = new ViewLevelException(GuidGenerator.CreateSimpleGuid())
        {
            Message = inputDto.Message,
            StackTrace = inputDto.StackTrace,
            HttpStatusCode = inputDto.HttpStatusCode
        };

        await _viewLevelExceptionsDbContext.ViewLevelException.AddAsync(viewLevelException);
        await _viewLevelExceptionsDbContext.SaveChangesAsync();

        return viewLevelException.Id;
    }
}
