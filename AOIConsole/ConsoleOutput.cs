using AOI.Core.Interfaces;
using System;

namespace AOIConsole
{
    public sealed class ConsoleOutput : IOutput
    {
        void IOutput.Output(string message)
        {
            Console.WriteLine(message);
        }
    }
}
