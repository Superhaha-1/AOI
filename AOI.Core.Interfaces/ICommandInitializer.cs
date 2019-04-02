namespace AOI.Core.Interfaces
{
    public interface ICommandInitializer
    {
        void InitializeCommand<T>(ICommandBuilder<T> commandBuilder) where T : IOperation, new();
    }
}
