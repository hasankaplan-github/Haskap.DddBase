using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Haskap.DddBase.Utilities.Module;
public interface IModule
{
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);
}
