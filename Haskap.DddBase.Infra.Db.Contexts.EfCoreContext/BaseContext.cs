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
    protected IGlobalQueryFilterParameterStatusCollectionProvider _globalQueryFilterParameterStatusCollectionProvider;


    private Guid? _currentTenantId => _currentTenantProvider.CurrentTenantId;
    private bool _multiTenancyFilterIsEnabled => _globalQueryFilterParameterStatusCollectionProvider.IsEnabled<IHasMultiTenant>();
    private bool _softDeleteFilterIsEnabled => _globalQueryFilterParameterStatusCollectionProvider.IsEnabled<ISoftDeletable>();

    protected BaseContext(
        DbContextOptions options,
        ICurrentTenantProvider currentTenantProvider,
        IGlobalQueryFilterParameterStatusCollectionProvider filterParameterStatusCollectionProvider)
        : base(options)
    {
        _currentTenantProvider = currentTenantProvider;
        _globalQueryFilterParameterStatusCollectionProvider = filterParameterStatusCollectionProvider;
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

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType mutableEntityType)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IHasMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return false;
    }

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
        {
            expression = x => !_softDeleteFilterIsEnabled || (x as ISoftDeletable).IsDeleted == false;
        }

        if (typeof(IHasMultiTenant).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>>? multiTenancyExpression = x => !_multiTenancyFilterIsEnabled || (x as IHasMultiTenant).TenantId == _currentTenantId;
            expression = expression == null ? multiTenancyExpression : CombineExpressions(expression, multiTenancyExpression);
        }

        return expression;
    }

    protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expression1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expression2.Body);

        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
    }

    class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
            {
                return _newValue;
            }

            return base.Visit(node);
        }
    }
}
