using AOI.Core.Interfaces;
using Autofac;

namespace AOI.Core.CommandManager
{
    public class CommandManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new CommandTextResolver(c.Resolve<ICommandInvoker>())).As<ICommandTextResolver>().SingleInstance();
            builder.Register(c => new CommandSchedule()).As<ICommandInitializer>().As<ICommandInvoker>().SingleInstance();
        }
    }
}
