using System.Net;
using Haskap.DddBase.Domain.Shared.Resources;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class TooManyRequestsException : DomainException
{
    public TooManyRequestsException(int retryAfterSeconds)
        : base(string.Format(ExceptionMessages.TooManyRequestsException, retryAfterSeconds), HttpStatusCode.TooManyRequests)
    {

    }
}
