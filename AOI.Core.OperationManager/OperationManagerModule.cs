using AOI.Core.Interfaces;
using Autofac;

namespace AOI.Core.OperationManager
{
    public class OperationManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new OperationTextResolver(c.Resolve<IOperationInvoker>())).As<ICommandTextResolver>().SingleInstance();
            builder.Register(c => new OperationSchedule(c)).As<IOperationInitializer>().As<IOperationInvoker>().SingleInstance();
        }
    }
}
