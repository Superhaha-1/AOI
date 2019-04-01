using AOI.Core.Interfaces;
using Autofac;
using Splat.Autofac;
using System.IO;
using System.Linq;
using System.Reflection;
using Splat;
using Splat.Serilog;
using Serilog;
using Serilog.Core;
using Serilog.Events;

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
                Locator.CurrentMutable.UseSerilogFullLogger();
                using (var lifetimeScope = container.BeginLifetimeScope())
                {
                    var loggerConfiguration = new LoggerConfiguration().MinimumLevel.Verbose();
                    if (lifetimeScope.TryResolve<ILogEventSink>(out var logEventSink))
                        loggerConfiguration = loggerConfiguration.WriteTo.Sink(logEventSink);
                    Log.Logger = loggerConfiguration.WriteTo.SQLite("log.db", restrictedToMinimumLevel: LogEventLevel.Warning).CreateLogger();
                    lifetimeScope.Resolve<IAOIMain>().Run();
                    Log.CloseAndFlush();
                }
            }
        }
    }
}
