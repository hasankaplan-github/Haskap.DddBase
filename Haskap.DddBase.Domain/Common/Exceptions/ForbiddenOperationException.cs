using System.Net;
using Haskap.DddBase.Domain.Shared.Resources;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class ForbiddenOperationException : DomainException
{
    public ForbiddenOperationException(string permissionName)
        : base(string.Format(ExceptionMessages.ForbiddenOperationException, permissionName), HttpStatusCode.Forbidden)
    {

    }
}
