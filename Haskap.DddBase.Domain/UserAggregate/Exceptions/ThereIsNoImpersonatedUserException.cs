using System.Net;

namespace Haskap.DddBase.Domain.UserAggregate.Exceptions;
public class ThereIsNoImpersonatedUserException : DomainException
{
    public ThereIsNoImpersonatedUserException()
        : base("Başkası adına giriş yapılmamış durumda. Hatalı işlem.", HttpStatusCode.BadRequest)
    {

    }
}
