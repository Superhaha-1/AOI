using Autofac;
using System.Collections.Generic;

namespace AOI.Core.Interfaces
{
    public interface ICommandBuilder
    {
        ContainerBuilder ContainerBuilder { get; }

        string Name { get; set; }

        IDictionary<string, ICommandParameterBuilder> ParameterBuilderDictionary { get; }
    }

    public interface ICommandBuilder<T> : ICommandBuilder where T : IOperation, new()
    {

    }
}
