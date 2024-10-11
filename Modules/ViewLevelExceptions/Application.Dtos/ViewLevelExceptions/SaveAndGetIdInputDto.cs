using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ViewLevelExceptions.Application.Dtos.ViewLevelExceptions;
public class SaveAndGetIdInputDto
{
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}
