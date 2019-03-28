using AOI.Core.Interfaces;
using System;

namespace AOI.Core.CommandManager
{
    public sealed class CommandTextResolver : ICommandTextResolver
    {
        void ICommandTextResolver.Resolve(string commandText)
        {
            Console.WriteLine(commandText);
        }
    }
}