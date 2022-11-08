using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Haskap.DddBase.Domain;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Haskap.DddBase.Domain.Providers;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;

public class BaseContext : DbContext
{
    protected ICurrentTenantProvider _currentTenantProvider;
    private Guid? _currentTenantId => _currentTenantProvider.CurrentTenantId;
    private bool _multiTenancyIsEnabled => _currentTenantProvider.MultiTenancyIsEnabled;

    //public BaseContext(
    //    DbContextOptions<BaseContext> options,
    //    ICurrentTenantProvider currentTenantProvider)
    //    : base(options)
    //{
    //    _currentTenantProvider = currentTenantProvider;
    //}

    protected BaseContext(
        DbContextOptions options, 
        ICurrentTenantProvider currentTenantProvider)
        : base(options)
    {
        _currentTenantProvider = currentTenantProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    }



    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
       = typeof(BaseContext)
           .GetMethod(
               nameof(ConfigureBaseProperties),
               BindingFlags.Instance | BindingFlags.NonPublic
           );

    
    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        //if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        //{
        //    return;
        //}

        //modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }

    protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
        {
            var filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null)
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
            }
        }
    }

    private bool _isSoftDeletable = false;
    private bool _hasMultiTenant = false;

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType mutableEntityType)
    {
        _isSoftDeletable = false;
        _hasMultiTenant = false;

        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            _isSoftDeletable = true;
        }

        if (typeof(IHasMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            _hasMultiTenant = true;
        }

        if (_isSoftDeletable || _hasMultiTenant)
        {
            return true;
        }

        return false;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
    {
        return (x => 
        ((!_isSoftDeletable || (x as ISoftDeletable).IsDeleted == false) &&
         (!_hasMultiTenant || !_multiTenancyIsEnabled || (x as IHasMultiTenant).TenantId == _currentTenantId)));
    }

}
