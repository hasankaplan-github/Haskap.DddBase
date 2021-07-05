using MediatR;
using System.Collections.Generic;

namespace Haskap.DddBase.Domain.Core
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
