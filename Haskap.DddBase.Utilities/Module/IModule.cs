using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public interface IModule
{
    string ModuleName { get; }
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);
    Task ThrowIfDisabledAsync(string requestPath, CancellationToken cancellationToken = default);
}
