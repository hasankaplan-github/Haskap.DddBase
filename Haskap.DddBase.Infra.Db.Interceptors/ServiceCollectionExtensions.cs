using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Interceptors;
public static class ServiceCollectionExtensions
{
    public static void AddBaseInterceptors(this IServiceCollection services)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<AuditHistoryLogSaveChangesInterceptor>();
        services.AddScoped<MultiTenancySaveChangesInterceptor>();
    }

    public static void AddBaseInterceptors(this DbContextOptionsBuilder options, IServiceProvider serviceProvider)
    {
        options.AddInterceptors(
            serviceProvider.GetRequiredService<MultiTenancySaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor>(),
            serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
    }
}