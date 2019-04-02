using System;

namespace AOI.Core.Interfaces
{

    public interface ICommandParameterBuilder
    {
        ICommandBuilder CommandBuilder { get; }

        string Name { get; set; }

        string HelpMessage { get; set; }

        void Set(IOperation operation, object parameter);
    }

    public interface ICommandParameterBuilder<T, M> : ICommandParameterBuilder where T : IOperation, new()
    {
        Action<T, M> Action { set; }
    }
}
