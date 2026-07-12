namespace Haskap.DddBase.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
