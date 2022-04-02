using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.DddBase.Utilities;

public class DisposeAction : IDisposable
{
    private readonly Action _action;

    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposeAction(Action action)
    {
        _action = action;
    }

    public void Dispose()
    {
        if (_action != null)
        {
            _action();
        }
    }
}
