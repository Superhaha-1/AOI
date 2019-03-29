using Autofac;
using AOI.Core.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace AOIConsole
{
    public class AOIConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new AOIMain(c.Resolve<ICommandTextResolver>())).As<IAOIMain>().InstancePerLifetimeScope();
            builder.Register(c => new LogEventSink()).Named<ILogEventSink>(nameof(LogEventLevel.Information)).InstancePerLifetimeScope();
        }
    }
}
