using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class EmailAddressIsEmptyException : DomainException
{
    public EmailAddressIsEmptyException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
