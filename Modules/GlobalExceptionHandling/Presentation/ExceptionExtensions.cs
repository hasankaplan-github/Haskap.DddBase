using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Modules.Localization.Application.Contracts;
using System.Net;

namespace Modules.GlobalExceptionHandling.Presentation;
public static class ExceptionExtensions
{
    public static Envelope ToEnvelope(this Exception exception, HttpContext httpContext)
    {
        string localizedMessage = exception.Message;
        var httpStatusCode = HttpStatusCode.BadRequest;

        if (exception is DomainException domainException)
        {
            LocalizedString localizedString = new LocalizedString(domainException.LocalizationKey, domainException.Message, resourceNotFound: true);
            var stringLocalizerFactory = httpContext.RequestServices.GetRequiredService<IStringLocalizerFactory>();

            foreach (var resourceType in AppConfig.LocalizationResourceTypes)
            {
                //var stringLocalizer = stringLocalizerFactory.Create("Resources.ExceptionMessages", "Haskap.DddBase.Domain.Shared");
                var stringLocalizer = stringLocalizerFactory.Create(resourceType);
                if (stringLocalizer is null)
                {
                    continue;
                }

                localizedString = stringLocalizer[domainException.LocalizationKey, domainException.LocalizationParams];
                if (!localizedString.ResourceNotFound)
                {
                    break;
                }
            }

            if (localizedString.ResourceNotFound)
            {
                var dbStringLocalizer = httpContext.RequestServices.GetService<IDbStringLocalizer>();

                if (dbStringLocalizer is not null)
                {
                    localizedString = dbStringLocalizer[domainException.LocalizationKey, domainException.LocalizationParams];
                }
            }

            localizedMessage = localizedString.ResourceNotFound ? localizedString.Name + ":" + localizedString.Value : localizedString.Value;
            httpStatusCode = domainException.HttpStatusCode;
        }

        var errorEnvelope = Envelope.Error(localizedMessage, exception.StackTrace, exception.GetType().ToString(), httpStatusCode);
        
        return errorEnvelope;
    }
}
