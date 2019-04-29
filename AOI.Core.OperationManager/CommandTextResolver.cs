using AOI.Core.Interfaces;
using CommandLine;
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
            //int i = 0;
            //int length = commandText.Length;
            //while (i < length)
            //{
            //    if (commandText[i] == '-')
            //    {
            //        break;
            //    }
            //    i++;
            //}
            //if (i == length)
            //{
            //    this.Log().Info($"命令-{commandText}-无法识别");
            //    return;
            //}
            //var operationName = new StringBuilder();
            //i++;
            //while (i < length)
            //{
            //    var c = commandText[i];
            //    if (c == ' ')
            //    {
            //        break;
            //    }
            //    operationName.Append(c);
            //    i++;
            //}
            //if(operationName.Length == 0)
            //{
            //    this.Log().Info("无法解析操作");
            //    return;
            //}
            //_operationInvoker.InvokeOperation(operationName.ToString());
        }
    } 
}