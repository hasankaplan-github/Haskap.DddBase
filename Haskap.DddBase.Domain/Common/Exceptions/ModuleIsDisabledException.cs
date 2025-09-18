using Haskap.DddBase.Domain.Shared.Resources;
using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class ModuleIsDisabledException : DomainException
{
    public ModuleIsDisabledException(string moduleName, string requestPath)
        : base(string.Format(ExceptionMessages.ModuleIsDisabledException, moduleName, requestPath), HttpStatusCode.FailedDependency)
    {

    }
}
