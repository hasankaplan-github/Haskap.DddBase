using System.Net;

namespace Haskap.DddBase.Domain.UserAggregate.Exceptions;
public class ForbiddenOperationException : DomainException
{
    public ForbiddenOperationException(string permissionName)
        : base($"""Bu işlem için "{permissionName}" yetkisine sahip olmanız gerekmektedir! Yöneticiniz ile görüşünüz.""", HttpStatusCode.Forbidden)
    {

    }
}
