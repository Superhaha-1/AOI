using AOI.Core.Interfaces;
using Autofac;
using Splat.Autofac;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace AOI
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(Directory.GetFiles("DirectoryModules").Select(path => Assembly.LoadFile(Path.GetFullPath(path))).ToArray());
            using (var container = containerBuilder.Build())
            {
                container.UseAutofacDependencyResolver();
                using (var lifetimeScope = container.BeginLifetimeScope())
                {
                    lifetimeScope.Resolve<IAOICore>().Run();
                }
            }
        }
    }
}
