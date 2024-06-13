using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;
public static class ServiceCollectionExtensions
{
    public static void AddBaseDomainServices(this IServiceCollection services)
    {
        //services.AddTransient<UserDomainService>();
    }
}