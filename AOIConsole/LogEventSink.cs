using Serilog.Core;
using Serilog.Events;
using System;

namespace AOIConsole
{
    public sealed class LogEventSink : ILogEventSink
    {
        void ILogEventSink.Emit(LogEvent logEvent)
        {
            Console.WriteLine(logEvent.RenderMessage());
        }
    }
}
