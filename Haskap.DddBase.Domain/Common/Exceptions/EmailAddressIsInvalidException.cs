using Haskap.DddBase.Domain;
using System.Net;

namespace Haskap.DddBase.Domain.Common.Exceptions;
public class EmailAddressIsInvalidException : DomainException
{
    public EmailAddressIsInvalidException()
        : base(HttpStatusCode.BadRequest)
    {

    }
}
