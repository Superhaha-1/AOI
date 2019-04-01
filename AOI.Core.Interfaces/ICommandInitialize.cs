using System;
using System.Collections.Generic;

namespace AOI.Core.Interfaces
{
    public interface ICommandInitialize
    {
        void InitializeCommand<T>(string commandName, IDictionary<string, Action<T, object>> setters) where T : IOperation, new();
    }
}
