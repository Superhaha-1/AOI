using System.Collections.Generic;
using System.ComponentModel;

namespace AOI.Core.Interfaces
{
    public interface IOperationBuilder
    {
        string Name { get; }

        string Description { get; }

        IOperation CreateOperation();

        IDictionary<string, IOperationParameterBuilder> ParameterBuilderDictionary { get; }
    }
}
