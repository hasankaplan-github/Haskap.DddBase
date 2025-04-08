using Haskap.DddBase.Domain;
using Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ViewLevelExceptions.Domain.ViewLevelExceptionAggregate;
public class ViewLevelException : AggregateRoot<Guid>
{
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime OccuredOnUtc { get; private set; }
    public HttpStatusCode HttpStatusCode { get; set; }


    private ViewLevelException()
    { }

    public ViewLevelException(Guid id)
        : base(id)
    {
        OccuredOnUtc = DateTime.UtcNow;
    }

    public ViewLevelExceptionOutputDto ToViewLevelExceptionOutputDto()
    {
        return new()
        {
            Id = Id,
            Message = Message,
            StackTrace = StackTrace,
            OccuredOnUtc = OccuredOnUtc,
            HttpStatusCode = HttpStatusCode
        };
    }
}
