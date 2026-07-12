using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared;
using Haskap.DddBase.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Haskap.DddBase.Infra.Db.Contexts.EfCoreContext;

public class BaseContext : DbContext
{
    protected ICurrentTenantProvider? CurrentTenantProvider;
    protected IGlobalQueryFilterManagerProvider? GlobalQueryFilterManagerProvider;


    private Guid? _currentTenantId => CurrentTenantProvider?.CurrentTenantId;
    private bool _multiTenancyFilterIsEnabled => GlobalQueryFilterManagerProvider?.IsEnabled<IHasMultiTenant>() ?? false;
    private bool _softDeleteFilterIsEnabled => GlobalQueryFilterManagerProvider?.IsEnabled<ISoftDeletable>() ?? false;
    private bool _isActiveFilterIsEnabled => GlobalQueryFilterManagerProvider?.IsEnabled<IIsActive>() ?? false;


    protected BaseContext(
        DbContextOptions options,
        ICurrentTenantProvider? currentTenantProvider,
        IGlobalQueryFilterManagerProvider? globalQueryFilterManagerProvider)
        : base(options)
    {
        CurrentTenantProvider = currentTenantProvider;
        GlobalQueryFilterManagerProvider = globalQueryFilterManagerProvider;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseContext).Assembly);

        //TrySetDatabaseProvider(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureBasePropertiesMethodInfo
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(this, new object[] { modelBuilder, entityType });
        }
    }



    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
       = typeof(BaseContext).GetMethod(
               nameof(ConfigureBaseProperties),
               BindingFlags.Instance | BindingFlags.NonPublic
           )!;

    
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
            var filterExpressions = CreateFilterExpressions<TEntity>();
            foreach (var filterExpression in filterExpressions)
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression.Key, filterExpression.Value);
            }
        }
    }

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType mutableEntityType)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IHasMultiTenant).IsAssignableFrom(typeof(TEntity)) && AppConfig.IsMultiTenant && AppConfig.MultiTenancyType == MultiTenancyType.SharedDb)
        {
            return true;
        }

        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return false;
    }

    protected virtual IDictionary<string, Expression<Func<TEntity, bool>>> CreateFilterExpressions<TEntity>()
    {
        IDictionary<string, Expression<Func<TEntity, bool>>> filterExpressions = new Dictionary<string, Expression<Func<TEntity, bool>>>();

        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            var softDeleteFilterProvider = GlobalQueryFilterManagerProvider?.GetFilterProvider<ISoftDeletable>();
            if (softDeleteFilterProvider is not null)
            {
                Expression<Func<TEntity, bool>> softDeleteFilterExpression = x => !_softDeleteFilterIsEnabled || (x as ISoftDeletable).IsDeleted == false;
                filterExpressions.Add(softDeleteFilterProvider!.Key, softDeleteFilterExpression);
            }
        }

        if (typeof(IHasMultiTenant).IsAssignableFrom(typeof(TEntity)) && AppConfig.IsMultiTenant && AppConfig.MultiTenancyType == MultiTenancyType.SharedDb)
        {
            var multiTenancyFilterProvider = GlobalQueryFilterManagerProvider?.GetFilterProvider<IHasMultiTenant>();
            if (multiTenancyFilterProvider is not null)
            {
                Expression<Func<TEntity, bool>> multiTenancyFilterExpression = x => !_multiTenancyFilterIsEnabled || (x as IHasMultiTenant).TenantId == _currentTenantId;
                filterExpressions.Add(multiTenancyFilterProvider!.Key, multiTenancyFilterExpression);
            }
        }

        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
        {
            var isActiveFilterProvider = GlobalQueryFilterManagerProvider?.GetFilterProvider<IIsActive>();
            if (isActiveFilterProvider is not null)
            {
                Expression<Func<TEntity, bool>> isActiveFilterExpression = e => !_isActiveFilterIsEnabled || (e as IIsActive).IsActive == true; // EF.Property<bool>(e, "IsActive");
                filterExpressions.Add(isActiveFilterProvider!.Key, isActiveFilterExpression);
            }
        }

        return filterExpressions;
    }

    //protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    //{
    //    var parameter = Expression.Parameter(typeof(T));

    //    var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
    //    var left = leftVisitor.Visit(expression1.Body);

    //    var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
    //    var right = rightVisitor.Visit(expression2.Body);

    //    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    //}

    //class ReplaceExpressionVisitor : ExpressionVisitor
    //{
    //    private readonly Expression _oldValue;
    //    private readonly Expression _newValue;

    //    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    //    {
    //        _oldValue = oldValue;
    //        _newValue = newValue;
    //    }

    //    public override Expression Visit(Expression node)
    //    {
    //        if (node == _oldValue)
    //        {
    //            return _newValue;
    //        }

    //        return base.Visit(node);
    //    }
    //}
}
