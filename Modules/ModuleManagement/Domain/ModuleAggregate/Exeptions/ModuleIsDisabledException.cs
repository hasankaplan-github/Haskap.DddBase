using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Shared.Resources;
using System.Net;

namespace Modules.ModuleManagement.Domain.ModuleAggregate.Exceptions;
public class ModuleIsDisabledException : DomainException
{
    public ModuleIsDisabledException(string moduleName, string requestPath)
        : base(string.Format(ExceptionMessages.ModuleIsDisabledException, moduleName, requestPath), HttpStatusCode.FailedDependency)
    {

    }
}
