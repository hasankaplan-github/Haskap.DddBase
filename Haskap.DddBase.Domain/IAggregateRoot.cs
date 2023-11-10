using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Domain;

public interface IAggregateRoot<TId> : IEntity<TId>, IAggregateRoot
{
}

public interface IAggregateRoot { }
