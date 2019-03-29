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
            this.Log().Debug("Debug");
            this.Log().Info("AOI正在运行");
            this.Log().Error("Error");
            while (true)
            {
                _commandTextResolver.Resolve(Console.ReadLine());
            }
        }
    }
}
