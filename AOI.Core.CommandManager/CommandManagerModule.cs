using AOI.Core.Interfaces;
using Autofac;

namespace AOI.Core.CommandManager
{
    public class CommandManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new CommandTextResolver()).As<ICommandTextResolver>().InstancePerLifetimeScope();
            builder.Register(c => new CommandInitialize()).As<ICommandInitialize>().InstancePerLifetimeScope();
        }
    }
}
