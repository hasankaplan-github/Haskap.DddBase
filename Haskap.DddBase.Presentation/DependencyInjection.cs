using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Common.Exceptions;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.RateLimiting;

namespace Haskap.DddBase.Presentation;
public static class DependencyInjection
{
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