using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Common.Exceptions;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modules.GlobalExceptionHandling.Application.Contracts;

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
        var globalExceptionHandlingModule = httpContext.RequestServices.GetRequiredService<IGlobalExceptionHandlingModule>();
        if(!await globalExceptionHandlingModule.IsEnabledAsync(httpContext.FindTenantId(), cancellationToken))
        {
            exception = new ModuleIsDisabledException(globalExceptionHandlingModule.GetType().Name, httpContext.Request.Path.Value ?? string.Empty);
        }

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