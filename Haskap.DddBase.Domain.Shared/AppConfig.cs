using Haskap.DddBase.Domain.Shared.Enums;

namespace Haskap.DddBase.Domain.Shared;
public static class AppConfig
{
    public static MultiTenancyType MultiTenancyType { get; set; } = MultiTenancyType.SharedDb;
    public static bool IsMultiTenant { get; set; } = false;
}
