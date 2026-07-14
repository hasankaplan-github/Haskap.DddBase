using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Common.Exceptions;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.GlobalExceptionHandling.Domain.Shared;
using System.Text.Json;

namespace Modules.GlobalExceptionHandling.Presentation;

public class LoggingExceptionHandler : IExceptionHandler
{
    private readonly ILogger<LoggingExceptionHandler> _logger;

    public LoggingExceptionHandler(ILogger<LoggingExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var globalExceptionHandlingModule = httpContext.RequestServices.GetRequiredService<IGlobalExceptionHandlingModule>();
        if (!await globalExceptionHandlingModule.IsEnabledAsync(httpContext.FindTenantId(), cancellationToken))
        {
            exception = new ModuleIsDisabledException(globalExceptionHandlingModule.GetType().Name, httpContext.Request.Path.Value ?? string.Empty);
        }

        var errorEnvelope = exception.ToEnvelope(httpContext);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        };

        var logMessage = 
            $"""
            =====================================================================================================================
            {JsonSerializer.Serialize<Envelope>(errorEnvelope, jsonSerializerOptions)}
            =====================================================================================================================
            """;
        _logger.LogError(logMessage);

        httpContext.Items["Exception"] = exception;
        httpContext.Items["Envelope"] = errorEnvelope;

        return false;
    }
}