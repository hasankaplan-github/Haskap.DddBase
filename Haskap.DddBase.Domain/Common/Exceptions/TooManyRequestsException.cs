using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class TooManyRequestsException : DomainException
{
    public TooManyRequestsException(int retryAfterSeconds)
        : base(HttpStatusCode.TooManyRequests, retryAfterSeconds)
    {

    }
}
