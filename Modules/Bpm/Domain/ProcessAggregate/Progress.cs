namespace Modules.Bpm.Domain.ProcessAggregate;

public class Progress : Entity
{
    public Guid RequestId { get; private set; }
    public Guid PathId { get; private set; }
    public DateTime UtcProgressDate { get; private set; }
    public Guid? OwnerUserId { get; private set; }
    public Guid? DataId { get; set; }

    private Progress()
    {
    }

    internal Progress(Guid id, Path path, Guid? ownerUserId, Guid? dataId)
        : base(id)
    {
        PathId = path.Id;
        UtcProgressDate = DateTime.UtcNow;
        OwnerUserId = ownerUserId;
        DataId = dataId;
    }
}