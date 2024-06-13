using Haskap.DddBase.Application.Contracts.Tenants;
using Haskap.DddBase.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Application.UseCaseServices.Tenants;
using Haskap.DddBase.Application.UseCaseServices.ViewLevelExceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Application.UseCaseServices;
public static class ServiceCollectionExtensions
{
    public static void AddBaseServices(this IServiceCollection services)
    {
        services.AddTransient<ITenantService, TenantService>();
        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();
    }
}