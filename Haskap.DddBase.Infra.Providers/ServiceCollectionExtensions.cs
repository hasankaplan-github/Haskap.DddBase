using Haskap.DddBase.Domain.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Providers;
public static class ServiceCollectionExtensions
{
    public static void AddBaseProviders<TUserId>(this IServiceCollection services)
    {
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        //services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<ICurrentUserIdProvider<TUserId>, CurrentUserIdProvider<TUserId>>();
        services.AddScoped<IVisitIdProvider, VisitIdProvider>();
    }
}