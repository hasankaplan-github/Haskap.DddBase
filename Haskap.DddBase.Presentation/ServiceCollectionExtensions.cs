using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Presentation;
public static class ServiceCollectionExtensions
{
    public static void AddBaseCustomAuthorization(this IServiceCollection services, IPermissionProvider permissionProvider)
    {
        services.AddSingleton<IPermissionProvider>(permissionProvider);
        services.AddAuthorization(permissionProvider.ConfigureAuthorization);
    }
}