﻿using Haskap.DddBase.Modules.ViewLevelExceptions.Application.Contracts.ViewLevelExceptions;
using Haskap.DddBase.Modules.ViewLevelExceptions.Application.UseCaseServices.ViewLevelExceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Modules.ViewLevelExceptions.Application.UseCaseServices;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCaseServices(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();

        return services;
    }
}
