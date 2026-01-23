using Haskap.DddBase.Application;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Modules.Bpm.Application.Contracts;
using Modules.Bpm.Application.Dtos;
using Modules.Bpm.Application.Mappings;
using Modules.Bpm.Domain;
using Modules.Bpm.Domain.ProcessAggregate;

namespace Modules.Bpm.Application;

internal class ProcessService : UseCaseService, IProcessService
{
    private readonly ProcessDomainService _processDomainService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IBpmDbContext _bpmDbContext;

    public ProcessService(
        ProcessDomainService processDomainService,
        ICurrentUserIdProvider currentUserIdProvider,
        IBpmDbContext bpmDbContext)
    {
        _processDomainService = processDomainService;
        _currentUserIdProvider = currentUserIdProvider;
        _bpmDbContext = bpmDbContext;
    }

    public async Task<Guid> InitRequestAsync(InitRequestInputDto input, CancellationToken cancellationToken = default)
    {
        var requestId = await _processDomainService.InitRequestAsync(input.ProcessId, _currentUserIdProvider.CurrentUserId, input.DataId, cancellationToken);

        return requestId;
    }

    public async Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        await _bpmDbContext.Request
            .Where(x => x.Id == requestId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AvailablePathOutputDto>> GetAvailablePathsAsync(GetAvailablePathsInputDto input, CancellationToken cancellationToken = default)
    {
        var process = await _bpmDbContext.Process
            .AsNoTracking()
            .Include(x => x.Requests.Where(y => y.Id == input.RequestId))
            .Include(x => x.Paths.Where(y =>
                y.FromStateId == x.Requests.Where(y => y.Id == input.RequestId).First().CurrentStateId &&
                (y.Roles.Any(role => role.RoleId == null) ||
                y.Roles.Any(z => input.UserRoleIds.Contains(z.RoleId!.Value)))))
                .ThenInclude(x => x.Command)
            .Where(x => x.Id == input.ProcessId)
            .FirstAsync(cancellationToken);

        var availablePaths = process.GetAvailablePathsOfRequest(input.RequestId, input.UserRoleIds);

        return availablePaths.Select(x => x.ToAvailablePathOutputDto()).ToList().AsReadOnly();
    }

    public async Task<Guid> MakeProgressAsync(MakeProgressInputDto input, CancellationToken cancellationToken = default)
    {
        var process = await _bpmDbContext.Process
            .Include(x => x.Requests.Where(y => y.Id == input.RequestId))
            .Include(x => x.Paths.Where(y =>
                y.FromStateId == x.Requests.Where(y => y.Id == input.RequestId).First().CurrentStateId &&
                y.CommandId == input.CommandId &&
                (y.Roles.Any(role => role.RoleId == null) ||
                y.Roles.Any(z => input.UserRoleIds.Contains(z.RoleId!.Value)))))
            .Where(x => x.Id == input.ProcessId)
            .FirstAsync(cancellationToken);

        var progressId = process.MakeProgress(input.RequestId, input.CommandId, input.UserRoleIds, _currentUserIdProvider.CurrentUserId, input.DataId, cancellationToken);

        return progressId;
    }
}
