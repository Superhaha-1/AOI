using System.ComponentModel;

namespace AOI.Core.Interfaces
{
    public interface IOperationParameterBuilder
    {
        string Name { get; }

        string Description { get; }

        void SetParameter(IOperation operation, object parameter);
    }
}
