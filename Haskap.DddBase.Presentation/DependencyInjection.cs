using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Haskap.DddBase.Domain.Shared.Consts;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Common.Exceptions;
using System.Globalization;

namespace Haskap.DddBase.Presentation;
public static class DependencyInjection
{
    public static void AddBaseCustomAuthorization(this IServiceCollection services, IPermissionProvider permissionProvider, Type accountServiceType)
    {
        services.AddSingleton<IPermissionProvider>(permissionProvider);
        services.AddAuthorization(permissionProvider.ConfigureAuthorization);

        services.AddSingleton<IAuthorizationHandler>(serviceProvider =>
            new PermissionAuthorizationHandler(
                serviceProvider.GetRequiredService<IServiceScopeFactory>(),
                accountServiceType));

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }

    public static void AddIpAddressRateLimiterPolicy(this IServiceCollection services, IpAddressRateLimiterOptions? limiterOptions = null)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.AddPolicy<IPAddress>(
                policyName: RateLimiterConsts.IpAddressRateLimiterPolicyName,
                partitioner: httpContext =>
                {
                    IPAddress? remoteIpAddress = httpContext.Connection.RemoteIpAddress;

                    if (IPAddress.IsLoopback(remoteIpAddress!))
                    {
                        return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                    }

                    limiterOptions ??= new IpAddressRateLimiterOptions();

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: remoteIpAddress!,
                        factory: _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = limiterOptions.PermitLimit,
                            Window = limiterOptions.Window
                        });
                });

            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.OnRejected = async (onRejectedContext, cancellationToken) =>
            {
                onRejectedContext.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter);

                throw new TooManyRequestsException((int)retryAfter.TotalSeconds);
            };
        });
    }
}