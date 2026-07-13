using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;

namespace Haskap.DddBase.Presentation;
public static class RateLimiterExtension
{
    public static void AddIpAddressGlobalRateLimiterPolicy(this IServiceCollection services, IpAddressRateLimiterOptions? limiterOptions = null)
    {
        limiterOptions ??= new IpAddressRateLimiterOptions();

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: partitionKey => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = limiterOptions.PermitLimit,
                        Window = limiterOptions.Window
                    }));

            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.OnRejected = async (onRejectedContext, cancellationToken) =>
            {
                var retryAfterTotalSeconds = (int)limiterOptions.Window.TotalSeconds;

                if (onRejectedContext.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    retryAfterTotalSeconds = (int)retryAfter.TotalSeconds;
                }

                throw new TooManyRequestsException(retryAfterTotalSeconds);
            };
        });
    }
}