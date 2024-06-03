namespace Haskap.DddBase.Domain
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
