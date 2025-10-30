using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Common.Exceptions;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.ModuleManagement.Application.Contracts.Module;

public abstract class BaseModule<TModule> : IModule
    where TModule : class, IModule
{
    protected readonly IModuleService ModuleService;
    protected readonly ICurrentTenantProvider CurrentTenantProvider;

    public string ModuleName => typeof(TModule).Name;

    public BaseModule(
        IModuleService moduleService,
        ICurrentTenantProvider currentTenantProvider)
    {
        ModuleService = moduleService;
        CurrentTenantProvider = currentTenantProvider;
    }

    public virtual async Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await ModuleService.IsEnabledAsync<TModule>(CurrentTenantProvider.CurrentTenantId, cancellationToken);
    }

    public virtual async Task<bool> IsEnabledAsync(Guid? tenantId, CancellationToken cancellationToken = default)
    {
        return await ModuleService.IsEnabledAsync<TModule>(tenantId, cancellationToken);
    }

    public virtual async Task ThrowIfDisabledAsync(string requestPath, CancellationToken cancellationToken = default)
    {
        if (!await IsEnabledAsync(cancellationToken))
        {
            throw new ModuleIsDisabledException(ModuleName, requestPath);
        }
    }

    public class DatabaseMigrator : IModuleDatabaseMigrator
    {
        public async Task MigrateAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            var domainAssemblyName = typeof(TModule).Namespace!.TrimEnd(['M', 'o', 'd', 'u', 'l', 'e']) + "Domain";
            var domainAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == domainAssemblyName);

            if (domainAssembly is null)
            {
                return;
            }

            var dbContextInterfaceTypes = domainAssembly.GetTypes().Where(t => t.IsInterface && t.GetInterface(typeof(IUnitOfWork).Name) != null) ?? Enumerable.Empty<Type>();
            foreach (var dbContextInterfaceType in dbContextInterfaceTypes)
            {
                var dbContext = serviceProvider.GetRequiredService(dbContextInterfaceType) as DbContext;
                await dbContext!.Database.MigrateAsync(cancellationToken);
            }
        }
    }
}
