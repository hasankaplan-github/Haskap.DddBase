using System.Net;

namespace Haskap.DddBase.Domain;
public abstract class DomainException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public string LocalizationKeyPrefix { get; protected set; } = "ExceptionMessage";
    public string ModuleName { get; protected set; }
    public string LocalizationKey => $"{LocalizationKeyPrefix}:{ModuleName}:{GetType().Name}";
    public object[] LocalizationParams { get; private set; }


    public DomainException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, params object[] args)
    {
        HttpStatusCode = httpStatusCode;
        ModuleName = GetModuleName();
        LocalizationParams = args;
    }

    public DomainException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, params object[] args)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
        ModuleName = GetModuleName();
        LocalizationParams = args;
    }

    public DomainException(string message, Exception inner, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, params object[] args)
        : base(message, inner)
    {
        HttpStatusCode = httpStatusCode;
        ModuleName = GetModuleName();
        LocalizationParams = args;
    }

    private string GetModuleName()
    {
        var typeNamespace = GetType().Namespace;

        return typeNamespace?.StartsWith("Modules.") == true
            ? typeNamespace.Split('.', 3)[1]
            : "Shared";
    }
}
