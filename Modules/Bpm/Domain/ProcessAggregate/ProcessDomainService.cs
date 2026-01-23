using Haskap.DddBase.Domain.Services;
using Haskap.DddBase.Utilities.Guids;
using Modules.Bpm.Domain.Shared.Enums;

namespace Modules.Bpm.Domain.ProcessAggregate;

public class ProcessDomainService : DomainService
{
    private readonly IBpmDbContext _bpmDbContext;

    public ProcessDomainService(IBpmDbContext bpmDbContext)
    {
        _bpmDbContext = bpmDbContext;
    }

    public async Task<Guid> InitRequestAsync(Guid processId, Guid? ownerUserId, Guid? dataId, CancellationToken cancellationToken)
    {
        var startState = _bpmDbContext.State
            .FirstOrDefault(x => x.ProcessId == processId && x.StateType == StateType.StartState);

        var newRequest = new Request(GuidGenerator.CreateSimpleGuid(), processId, ownerUserId, startState!, dataId);

        _bpmDbContext.Request.Add(newRequest);
        await _bpmDbContext.SaveChangesAsync(cancellationToken);

        return newRequest.Id;
    }
}
