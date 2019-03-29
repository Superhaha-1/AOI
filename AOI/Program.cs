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
using System;

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
                    foreach (var constant in Enum.GetValues(typeof(LogEventLevel)))
                    {
                        var logEventLevel = (LogEventLevel)constant;
                        lifetimeScope.TryResolveNamed(logEventLevel.ToString(), typeof(ILogEventSink), out object instance);
                        if (logEventLevel == LogEventLevel.Verbose)
                        {
                            loggerConfiguration = loggerConfiguration.WriteTo.Sink(instance as ILogEventSink ?? NullSink.Instance);
                        }
                        else
                        {
                            loggerConfiguration = loggerConfiguration.WriteTo.Sink(instance as ILogEventSink ?? NullSink.Instance, logEventLevel);
                        }
                    }
                    Log.Logger = loggerConfiguration.CreateLogger();
                    lifetimeScope.Resolve<IAOIMain>().Run();
                    Log.CloseAndFlush();
                }
            }
        }
    }
}
