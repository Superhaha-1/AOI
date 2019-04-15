using AOI.Core.Interfaces;
using Splat;

namespace AOI.Core.OperationManager
{
    public sealed class CommandTextResolver : ICommandTextResolver, IEnableLogger
    {
        private readonly IOperationInvoker _operationInvoker;

        public CommandTextResolver(IOperationInvoker operationInvoker)
        {
            _operationInvoker = operationInvoker;
        }

        void ICommandTextResolver.Resolve(string operationText)
        {
            operationText = operationText.Trim();
            var texts = operationText.Split(' ');
            if (texts.Length == 0)
            {
                this.Log().Info("没有可用的命令");
                return;
            }
            _operationInvoker.InvokeOperation(texts[0]);
        }
    }
}