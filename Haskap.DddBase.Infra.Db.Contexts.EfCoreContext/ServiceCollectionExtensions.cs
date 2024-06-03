using Haskap.DddBase.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;
public static class ServiceCollectionExtensions
{
    public static void AddBaseDbContext(this IServiceCollection services, Type interfaceDbContextType, Type implementationDbContextType)
    {
        services.AddScoped(typeof(IBaseDbContext), implementationDbContextType);
        services.AddScoped(interfaceDbContextType, implementationDbContextType);
    }
}