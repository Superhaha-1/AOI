using System.ComponentModel;

namespace AOI.Core.Interfaces
{
    public interface IOperationInvoker
    {
        void InvokeOperation(string name, params (string Name, object Value)[] parameters);
    }
}
