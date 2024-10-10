using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Tenants.Infra.Db.Interceptors;
public static class InterceptorExtensions
{
    public static void AddMultiTenancyInterceptors(this DbContextOptionsBuilder options, IServiceProvider serviceProvider)
    {
        options.AddInterceptors(serviceProvider.GetRequiredService<MultiTenancySaveChangesInterceptor>());
    }
}
