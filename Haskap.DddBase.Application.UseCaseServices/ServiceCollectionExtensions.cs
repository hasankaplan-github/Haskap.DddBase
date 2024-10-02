using Haskap.DddBase.Application.Contracts.ViewLevelExceptions;
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
        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();
    }
}