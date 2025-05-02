using Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;

namespace Modules.ViewLevelExceptions.Application.Mappings;
public static class DtoMappingExtensions
{
    public static ViewLevelExceptionOutputDto ToViewLevelExceptionOutputDto(this ViewLevelException viewLevelException)
    {
        return new()
        {
            Id = viewLevelException.Id,
            Message = viewLevelException.Message,
            StackTrace = viewLevelException.StackTrace,
            OccuredOnUtc = viewLevelException.OccuredOnUtc,
            HttpStatusCode = viewLevelException.HttpStatusCode
        };
    }
}
