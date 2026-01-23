using Haskap.DddBase.Application.Contracts;
using Modules.Bpm.Application.Dtos;

namespace Modules.Bpm.Application.Contracts;

public interface IProcessService : IUseCaseService
{
    Task<Guid> InitRequestAsync(InitRequestInputDto input, CancellationToken cancellationToken = default);
    Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AvailablePathOutputDto>> GetAvailablePathsAsync(GetAvailablePathsInputDto input, CancellationToken cancellationToken = default);
    Task<Guid> MakeProgressAsync(MakeProgressInputDto input, CancellationToken cancellationToken = default);
}
