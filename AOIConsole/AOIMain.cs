using AOI.Core.Interfaces;
using System;
using Splat;

namespace AOIConsole
{
    public sealed class AOIMain : IAOIMain, IEnableLogger
    {
        private ICommandTextResolver _commandTextResolver;

        public AOIMain(ICommandTextResolver commandTextResolver)
        {
            _commandTextResolver = commandTextResolver;
        }

        void IAOIMain.Run()
        {
            this.Log().Info("AOI正在运行");
            while (true)
            {
                _commandTextResolver.Resolve(Console.ReadLine());
            }
        }
    }
}
