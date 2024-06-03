﻿using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.DddBase.Domain.UserAggregate.Exceptions;
public class WrongUserNameOrPasswordException : DomainException
{
    public WrongUserNameOrPasswordException()
        : base("Kullanıcı Adı ya da Şifre hatalı!", HttpStatusCode.BadRequest)
    {

    }
}
