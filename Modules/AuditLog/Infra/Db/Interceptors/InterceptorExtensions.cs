using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.AuditLog.Infra.Db.Interceptors;
public static class InterceptorExtensions
{
    public static void AddAuditLogInterceptors(this DbContextOptionsBuilder options, IServiceProvider serviceProvider)
    {
        options.AddInterceptors(
            serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
    }
}
