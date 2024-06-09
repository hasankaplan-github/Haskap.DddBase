using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.UserAggregate;
using Haskap.DddBase.Domain.UserAggregate.Events;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Haskap.DddBase.Application.UseCaseServices.Accounts;
public class UserPermissionsCacheContentUpdatedEventHandler : INotificationHandler<UserPermissionsCacheContentUpdatedDomainEvent>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IBaseCacheKeyProvider _baseCacheKeyProvider;

    public UserPermissionsCacheContentUpdatedEventHandler(
        IMemoryCache memoryCache,
        IBaseCacheKeyProvider baseCacheKeyProvider)
    {
        _memoryCache = memoryCache;
        _baseCacheKeyProvider = baseCacheKeyProvider;
    }

    public async Task Handle(UserPermissionsCacheContentUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _memoryCache.Remove(_baseCacheKeyProvider.GetUserPermissionsCacheKey(notification.UserId));
        _memoryCache.Remove(_baseCacheKeyProvider.GetAllPermissionsCacheKey(notification.UserId));
    }
}
