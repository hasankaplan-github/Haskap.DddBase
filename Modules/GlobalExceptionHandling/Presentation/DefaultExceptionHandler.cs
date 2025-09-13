using Haskap.DddBase.Domain;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Modules.GlobalExceptionHandling.Presentation;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DefaultExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public DefaultExceptionHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DefaultExceptionHandler> logger,
        IWebHostEnvironment environment)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _environment = environment;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
        var exceptionStackTrace = exception.StackTrace;
        var httpStatusCode = exception switch
        {
            DomainException generalException => generalException.HttpStatusCode,
            _ => HttpStatusCode.BadRequest
        };
        var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception.GetType().ToString(), httpStatusCode);

        _logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{Environment.NewLine}" +
            $"=====================================================================================================================");

        if (!_environment.IsDevelopment())
        {
            errorEnvelope.SetExceptionStackTraceToNull();
        }

        httpContext.Response.StatusCode = (int)httpStatusCode;

        if (UtilityMethods.IsAjaxRequest(httpContext.Request))
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