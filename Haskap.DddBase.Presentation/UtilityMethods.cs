using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Haskap.DddBase.Presentation;

public class UtilityMethods
{
    public static bool IsAjaxRequest(HttpRequest request)
    {
        return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
    }
}
