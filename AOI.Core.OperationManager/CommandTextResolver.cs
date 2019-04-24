using AOI.Core.Interfaces;
using Splat;
using System.Text;

namespace AOI.Core.OperationManager
{
    public sealed class CommandTextResolver : ICommandTextResolver, IEnableLogger
    {
        private readonly IOperationInvoker _operationInvoker;

        public CommandTextResolver(IOperationInvoker operationInvoker)
        {
            _operationInvoker = operationInvoker;
        }

        void ICommandTextResolver.Resolve(string commandText)
        {
            int i = 0;
            int length = commandText.Length;
            while (i < length)
            {
                if (commandText[i] == '-')
                {
                    break;
                }
                i++;
            }
            if (i == length)
            {
                return;
            }
            var operationName = new StringBuilder();
            i++;
            while (i < length)
            {
                var c = commandText[i];
                if (c == ' ')
                {
                    break;
                }
                operationName.Append(c);
                i++;
            }
            _operationInvoker.InvokeOperation(operationName.ToString());
        }
    } 
}