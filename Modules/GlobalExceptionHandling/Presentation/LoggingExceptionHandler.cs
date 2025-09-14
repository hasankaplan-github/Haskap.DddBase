using Haskap.DddBase.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Modules.GlobalExceptionHandling.Presentation;

public class LoggingExceptionHandler : IExceptionHandler
{
    private readonly ILogger<LoggingExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public LoggingExceptionHandler(
        ILogger<LoggingExceptionHandler> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorEnvelope = Envelope.FromException(exception);

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

        return false;
    }
}