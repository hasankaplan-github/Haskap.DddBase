using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.UserAggregate;
using Haskap.DddBase.Domain.UserAggregate.Events;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Application.UseCaseServices.Accounts;
public class RolePermissionsCacheContentUpdatedEventHandler : INotificationHandler<RolePermissionsCacheContentUpdatedDomainEvent>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;
    private readonly IServiceProvider _serviceProvider;

    public RolePermissionsCacheContentUpdatedEventHandler(
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider,
        IServiceProvider serviceProvider)
    {
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task Handle(RolePermissionsCacheContentUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _memoryCache.Remove(_baseCacheKeyProvider.GetRolePermissionsCacheKey(notification.RoleId));

        using var scope = _serviceProvider.CreateScope();
        var baseDbContext = scope.ServiceProvider.GetRequiredService<IBaseDbContext>();

        var userIds = baseDbContext.UserRole
            .Where(x => x.RoleId == notification.RoleId)
            .Select(x => x.UserId)
            .ToList();

        foreach (var userId in userIds) 
        {
            _memoryCache.Remove(_baseCacheKeyProvider.GetAllPermissionsCacheKey(userId));
        }
    }
}
