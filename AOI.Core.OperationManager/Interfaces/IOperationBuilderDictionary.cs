using AOI.Core.Interfaces;
using System.Collections.Generic;

namespace AOI.Core.OperationManager.Interfaces
{
    public interface IOperationBuilderDictionary
    {
        IReadOnlyDictionary<string, IOperationBuilder> Operations { get; }
    }
}
