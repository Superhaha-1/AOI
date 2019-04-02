using Autofac;
using System.Collections.Generic;

namespace AOI.Core.Interfaces
{
    public interface ICommandBuilder
    {
        string Name { get; set; }

        IOperation CreateOperation();

        IDictionary<string, ICommandParameterBuilder> ParameterBuilderDictionary { get; }
    }
}
