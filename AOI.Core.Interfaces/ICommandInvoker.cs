namespace AOI.Core.Interfaces
{
    public interface ICommandInvoker
    {
        void InvokeCommand(string name, params (string Name, object Value)[] parameters);
    }
}
