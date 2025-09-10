using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;

namespace Modules.ModuleManagement.Domain.ModuleAggregate;

public class EnabledModule : AggregateRoot, IHasMultiTenant
{
    public string Name { get; private set; }
    public Guid? TenantId { get; set; }

    private EnabledModule()
    { }

    public EnabledModule(Guid id, string name)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
    }
}
