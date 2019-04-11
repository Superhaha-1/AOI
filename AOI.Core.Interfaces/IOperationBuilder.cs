using Autofac;
using System.Collections.Generic;

namespace AOI.Core.Interfaces
{
    public interface IOperationBuilder
    {
        string Name { get; }

        string Description { get; }

        IOperation CreateOperation(IComponentContext componentContext);

        IDictionary<string, IOperationParameterBuilder> ParameterBuilderDictionary { get; }
    }
}
