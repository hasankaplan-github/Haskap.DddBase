using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class ForbiddenOperationException : DomainException
{
    public ForbiddenOperationException(string permissionName)
        : base(HttpStatusCode.Forbidden, permissionName)
    {

    }
}
