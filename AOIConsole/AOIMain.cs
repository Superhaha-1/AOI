using AOI.Core.Interfaces;
using System;
using Splat;

namespace AOIConsole
{
    public sealed class AOIMain : IAOIMain, IEnableLogger
    {
        private readonly ICommandTextResolver _operationTextResolver;

        public AOIMain(ICommandTextResolver operationTextResolver)
        {
            _operationTextResolver = operationTextResolver;
        }

        void IAOIMain.Run()
        {
            this.Log().Info("AOI正在运行");
            while (true)
            {
                _operationTextResolver.Resolve(Console.ReadLine());
            }
        }
    }
}
