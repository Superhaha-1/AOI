using AOI.Core.Interfaces;
using Splat;
using System;
using System.Collections.Generic;

namespace AOI.Core.CommandManager
{
    public sealed class CommandTextResolver : ICommandTextResolver, IEnableLogger
    {
        private readonly ICommandInvoker _commandInvoker;

        public CommandTextResolver(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        void ICommandTextResolver.Resolve(string commandText)
        {
            commandText = commandText.Trim();
            var texts = commandText.Split(' ');
            if (texts.Length == 0)
            {
                this.Log().Info("没有可用的命令");
                return;
            }
            _commandInvoker.InvokeCommand(texts[0]);
        }
    }
}