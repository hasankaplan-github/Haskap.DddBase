using System.Net;

namespace Haskap.DddBase.Domain.UserAggregate.Exceptions;
public class AlreadyImpersonatedException : DomainException
{
    public AlreadyImpersonatedException()
        : base("Zaten başka bir kullanıcı adına giriş yapmış durumdasınız. Bu kullanıcı ile tekrar başkası adına giriş yapamazsınız.", HttpStatusCode.BadRequest)
    {

    }
}
