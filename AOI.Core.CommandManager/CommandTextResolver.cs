using AOI.Core.Interfaces;
using Splat;
using System;

namespace AOI.Core.CommandManager
{
    public sealed class CommandTextResolver : ICommandTextResolver, IEnableLogger
    {
        void ICommandTextResolver.Resolve(string commandText)
        {
            this.Log().Info($"正在解析命令\"{commandText}\"");
        }
    }
}