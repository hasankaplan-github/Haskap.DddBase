using Haskap.DddBase.Application;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Application.Mappings;
using Modules.ViewLevelExceptions.Domain;
using Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;

namespace Modules.ViewLevelExceptions.Application.ViewLevelExceptions;
public class ViewLevelExceptionService : UseCaseService, IViewLevelExceptionService
{
    private readonly IViewLevelExceptionsDbContext _viewLevelExceptionsDbContext;

    public ViewLevelExceptionService(
        IViewLevelExceptionsDbContext viewLevelExceptionsDbContext)
    {
        _viewLevelExceptionsDbContext = viewLevelExceptionsDbContext;
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

        var output = exception.ToViewLevelExceptionOutputDto();

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
