namespace Haskap.DddBase.Domain;

public interface IAggregateRoot<TId> : IEntity<TId>, IAggregateRoot
{
}

public interface IAggregateRoot { }
