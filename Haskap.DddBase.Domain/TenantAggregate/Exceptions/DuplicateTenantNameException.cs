using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Resources;
using Haskap.DddBase.Domain.Shared.Resources;

namespace Haskap.DddBase.Domain.TenantAggregate.Exceptions;
public class DuplicateTenantNameException : DomainException
{
    public DuplicateTenantNameException()
        : base(ExceptionMessages.DuplicateTenantNameException, HttpStatusCode.BadRequest)
    {
        //"Bu isim kayıtlı, farklı bir isim deneyin!"
    }
}
