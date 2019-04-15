using AOI.Core.Interfaces;
using AOI.Core.OperationManager.Interfaces;
using Autofac;
using AOI.Core.Extensions;
using AOI.Core.OperationManager.Operations;

namespace AOI.Core.OperationManager
{
    public class OperationManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new CommandTextResolver(c.Resolve<IOperationInvoker>())).As<ICommandTextResolver>().SingleInstance();
            builder.Register(c => new OperationSchedule(c.Resolve < ILifetimeScope >())).As<IOperationInitializer>().As<IOperationInvoker>().As<IOperationBuilderDictionary>().SingleInstance();
            builder.CreateOperationBuilder(c => new GetHelp(c.Resolve<IOperationBuilderDictionary>(), c.Resolve<IOutput>()), "Help", "获取命令帮助").Build();
        }
    }
}
