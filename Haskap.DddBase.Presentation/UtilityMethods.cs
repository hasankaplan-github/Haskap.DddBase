using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Haskap.DddBase.Presentation;

public class UtilityMethods
{
    public static bool IsAjaxRequest(HttpRequest request)
    {
        return request.Headers[HeaderNames.XRequestedWith] == "XMLHttpRequest";
    }
}
