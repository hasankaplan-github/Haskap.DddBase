using Haskap.DddBase.Domain;
using System.Net;

namespace Modules.Tenants.Domain.TenantAggregate.Exceptions;
public class DuplicateTenantNameException : DomainException
{
    public DuplicateTenantNameException()
        : base(HttpStatusCode.BadRequest)
    {
        //"Bu isim kayıtlı, farklı bir isim deneyin!"
    }
}
