using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;
public abstract class DomainException : Exception
{
    public DomainException(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        HttpStatusCode = httpStatusCode;
    }

    public DomainException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public DomainException(string message, Exception inner, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        : base(message, inner)
    {
        HttpStatusCode = httpStatusCode;
    }

    public HttpStatusCode HttpStatusCode { get; set; }
}
