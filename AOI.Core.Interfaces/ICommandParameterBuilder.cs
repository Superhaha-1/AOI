namespace AOI.Core.Interfaces
{
    public interface ICommandParameterBuilder
    {
        string Name { get; set; }

        string HelpMessage { get; set; }

        void Set(IOperation operation, object parameter);
    }
}
