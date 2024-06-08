using Haskap.DddBase.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Application.Dtos.ViewLevelExceptions;
using Haskap.DddBase.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting;

namespace Haskap.DddBase.Presentation.GlobalExceptionHandling;
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
            httpContext.Response.ContentType = Text.Plain;
            await httpContext.Response.WriteAsJsonAsync(errorEnvelope);
        }
        else
        {
            var inputDto = new SaveAndGetIdInputDto
            {
                Message = errorEnvelope.ExceptionMessage ?? string.Empty,
                StackTrace = errorEnvelope.ExceptionStackTrace
            };
            using var scope = _serviceScopeFactory.CreateScope();
            var viewLevelExceptionService = scope.ServiceProvider.GetRequiredService<IViewLevelExceptionService>();
            var errorId = await viewLevelExceptionService.SaveAndGetIdAsync(inputDto);

            httpContext.Response.Redirect($"/Home/Error?errorId={errorId}");
        }

        return true;
    }
}
