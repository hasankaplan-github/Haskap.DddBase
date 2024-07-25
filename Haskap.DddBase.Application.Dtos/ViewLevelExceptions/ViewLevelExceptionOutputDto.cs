using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.Dtos.ViewLevelExceptions;
public class ViewLevelExceptionOutputDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime OccuredOnUtc { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}
