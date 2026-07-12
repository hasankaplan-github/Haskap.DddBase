using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Infra.Interceptors;
public static class InterceptorExtensions
{
    public static void AddMultiTenancyInterceptors(this DbContextOptionsBuilder options, IServiceProvider serviceProvider)
    {
        options.AddInterceptors(serviceProvider.GetRequiredService<MultiTenancySaveChangesInterceptor>());
    }
}
