using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Shared.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Modules.GlobalExceptionHandling.Presentation;
public static class ExceptionExtensions
{
    public static Envelope ToEnvelope(this Exception exception, HttpContext httpContext)
    {
        string localizedMessage = exception.Message;
        var httpStatusCode = HttpStatusCode.BadRequest;

        if (exception is DomainException domainException)
        {
            var commonStringLocalizer = httpContext.RequestServices.GetRequiredService<ICommonStringLocalizer>();
            var dbStringLocalizer = httpContext.RequestServices.GetRequiredService<IDbStringLocalizer>();
            var stringLocalizerFactory = httpContext.RequestServices.GetRequiredService<IStringLocalizerFactory>();

            IList<IStringLocalizer> localizers = [];
            foreach (var resourceType in LocalizationResourceBase.LocalizationResourceTypes)
            {
                //var stringLocalizer = stringLocalizerFactory.Create("Resources.ExceptionMessages", "Haskap.DddBase.Domain.Shared");
                var stringLocalizer = stringLocalizerFactory.Create(resourceType);
                if (stringLocalizer is null)
                {
                    continue;
                }
                localizers.Add(stringLocalizer);
            }
            localizers.Add(dbStringLocalizer);
            commonStringLocalizer.SetStringLocalizers(localizers);

            localizedMessage = commonStringLocalizer[domainException.LocalizationKey, domainException.LocalizationParams];
            httpStatusCode = domainException.HttpStatusCode;
        }

        var errorEnvelope = Envelope.Error(localizedMessage, exception.StackTrace, exception.GetType().ToString(), httpStatusCode);
        
        return errorEnvelope;
    }
}
