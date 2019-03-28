using AOI.Core.Interfaces;
using System;

namespace AOIConsole
{
    public sealed class AOICore : IAOICore
    {
        private ICommandTextResolver _commandTextResolver;

        public AOICore(ICommandTextResolver commandTextResolver)
        {
            _commandTextResolver = commandTextResolver;
        }

        void IAOICore.Run()
        {
            while(true)
            {
                _commandTextResolver.Resolve(Console.ReadLine());
            }
        }
    }
}
