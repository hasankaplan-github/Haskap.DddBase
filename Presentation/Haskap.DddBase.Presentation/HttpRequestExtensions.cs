using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Haskap.DddBase.Presentation;
public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest? request)
    {
        return string.Equals(request?.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            string.Equals(request?.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
    }

}
