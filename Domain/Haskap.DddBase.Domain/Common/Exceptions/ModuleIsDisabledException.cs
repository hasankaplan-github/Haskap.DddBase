using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class ModuleIsDisabledException : DomainException
{
    public ModuleIsDisabledException(string moduleName, string requestPath)
        : base(HttpStatusCode.FailedDependency, moduleName, requestPath)
    {

    }
}
