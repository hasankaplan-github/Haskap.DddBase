namespace Haskap.DddBase.Domain.Common;
public class IpAddressRateLimiterOptions
{
    public int PermitLimit { get; set; } = 30;
    public TimeSpan Window { get; set; } = TimeSpan.FromSeconds(30);
}
