using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Http;
using Modules.CustomMessage.Application.Contracts;
using Modules.CustomMessage.Domain.Shared.Consts;

namespace Modules.CustomMessage.Presentation.Middlewares;

public class CustomMessageMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMessageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        ICustomMessageService customMessageService)
    {
        if (httpContext.Request.IsAjaxRequest() &&
            httpContext.Request.Headers[CustomMessageConsts.ResponseContainsCustomMessagesHeader] == "true")
        {
            httpContext.Response.Headers.Append(CustomMessageConsts.ResponseContainsCustomMessagesHeader, "true");

            // Store the original response stream
            var originalBodyStream = httpContext.Response.Body;

            // Use a MemoryStream to buffer the response
            using var newBodyStream = new MemoryStream();
            httpContext.Response.Body = newBodyStream;

            await _next.Invoke(httpContext);

            // After the pipeline, the response is in newBodyStream
            // Seek to the beginning to read it
            newBodyStream.Seek(0, SeekOrigin.Begin);
            using var newBodyStreamReader = new StreamReader(newBodyStream, leaveOpen: true);
            string originalResponse = await newBodyStreamReader.ReadToEndAsync();

            var modifiedResponse = $$"""{"data": {{(string.IsNullOrEmpty(originalResponse) ? "\"\"" : originalResponse)}}, "customMessages": {{customMessageService.GetAllMessagesAsJson()}}}""";

            // Write the modified response to the original stream
            using var modifiedStream = new MemoryStream();

            using var streamWriter = new StreamWriter(modifiedStream);
            streamWriter.Write(modifiedResponse);
            streamWriter.Flush();

            modifiedStream.Seek(0, SeekOrigin.Begin);
            await modifiedStream.CopyToAsync(originalBodyStream);

            // Restore the original response stream
            httpContext.Response.Body = originalBodyStream;
        }
        else
        {
            await _next.Invoke(httpContext);
        }
    }
}
