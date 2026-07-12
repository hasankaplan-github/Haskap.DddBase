namespace Haskap.DddBase.Utilities.Module;
public interface IModule
{
    string ModuleName { get; }
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);
    Task<bool> IsEnabledAsync(Guid? tenantId, CancellationToken cancellationToken = default);
    Task ThrowIfDisabledAsync(string requestPath, CancellationToken cancellationToken = default);
}
