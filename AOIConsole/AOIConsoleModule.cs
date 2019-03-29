using Autofac;
using AOI.Core.Interfaces;
using Serilog.Core;

namespace AOIConsole
{
    public class AOIConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new AOIMain(c.Resolve<ICommandTextResolver>())).As<IAOIMain>().InstancePerLifetimeScope();
            builder.Register(c => new ConsoleSink()).As<ILogEventSink>().InstancePerLifetimeScope();
        }
    }
}
