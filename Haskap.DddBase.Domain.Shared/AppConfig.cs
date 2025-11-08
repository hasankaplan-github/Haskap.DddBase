using Haskap.DddBase.Domain.Shared.Enums;
using Haskap.DddBase.Domain.Shared.Resources;

namespace Haskap.DddBase.Domain.Shared;
public static class AppConfig
{
    public static MultiTenancyType MultiTenancyType { get; set; } = MultiTenancyType.SharedDb;
    public static bool IsMultiTenant { get; set; } = false;
    public static IList<Type> LocalizationResourceTypes { get; set; } = [typeof(ExceptionMessages)];
}
