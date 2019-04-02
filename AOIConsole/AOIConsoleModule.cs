using Autofac;
using AOI.Core.Interfaces;
using Serilog.Core;
using Splat;
using AOI.Core.Extensions;

namespace AOIConsole
{
    public sealed class AOIConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new AOIMain(c.Resolve<ICommandTextResolver>())).As<IAOIMain>().SingleInstance();
            builder.Register(c => new ConsoleSink()).As<ILogEventSink>().SingleInstance();
            builder.CreateCommandBuilder<SignIn>().Build();
        }
    }

    public sealed class SignIn : IOperation, IEnableLogger
    {
        void IOperation.Redo()
        {
            this.Log().Info("已登录");
        }

        void IOperation.Revoke()
        {
        }
    }
}
