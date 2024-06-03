using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.DddBase.Domain.TenantAggregate.Exceptions;
public class DuplicateTenantNameException : DomainException
{
    public DuplicateTenantNameException()
        : base("Bu isim kayıtlı, farklı bir isim deneyin!", HttpStatusCode.BadRequest)
    {

    }
}
