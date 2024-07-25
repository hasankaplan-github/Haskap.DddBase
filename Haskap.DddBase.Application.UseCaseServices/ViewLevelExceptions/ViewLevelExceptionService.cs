using AutoMapper;
using Haskap.DddBase.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Application.Dtos.ViewLevelExceptions;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.ViewLevelExceptionAggregate;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices.ViewLevelExceptions;
public class ViewLevelExceptionService : UseCaseService, IViewLevelExceptionService
{
    private readonly IBaseDbContext _baseDbContext;
    private readonly IMapper _mapper;

    public ViewLevelExceptionService(
        IBaseDbContext baseDbContext,
        IMapper mapper)
    {
        _baseDbContext = baseDbContext;
        _mapper = mapper;
    }

    

    public async Task DeleteViewLevelExceptionAsync(Guid id)
    {
        await _baseDbContext.ViewLevelException
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<ViewLevelExceptionOutputDto> GetViewLevelExceptionAsync(Guid id)
    {
        var exception = await _baseDbContext.ViewLevelException
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

        await _baseDbContext.ViewLevelException.AddAsync(viewLevelException);
        await _baseDbContext.SaveChangesAsync();

        return viewLevelException.Id;
    }
}
