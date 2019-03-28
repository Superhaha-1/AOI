using Autofac;
using AOI.Core.Interfaces;

namespace AOIConsole
{
    public class AOIConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(c => new AOICore(c.Resolve<ICommandTextResolver>())).As<IAOICore>();
        }
    }
}
