using Haskap.DddBase.Domain;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Modules.GlobalExceptionHandling.Presentation;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly IWebHostEnvironment _environment;

    public DefaultExceptionHandler(IWebHostEnvironment environment)
    {
        _environment = environment;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorEnvelope = Envelope.FromException(exception);

        if (!_environment.IsDevelopment())
        {
            errorEnvelope.ClearExceptionStackTrace();
        }

        httpContext.Response.StatusCode = (int)errorEnvelope.HttpStatusCode;

        if (httpContext.Request.IsAjaxRequest())
        {
            // using static System.Net.Mime.MediaTypeNames;
            //httpContext.Response.ContentType = Text.Plain;
            await httpContext.Response.WriteAsJsonAsync(errorEnvelope);

            return true;
        }
        
        httpContext.Items["Exception"] = exception;
        httpContext.Items["Envelope"] = errorEnvelope;

        return false;
    }
}