using Serilog.Core;
using Serilog.Events;

namespace AOI
{
    public sealed class NullSink : ILogEventSink
    {
        public static NullSink Instance { get; } = new NullSink();

        private NullSink()
        {

        }

        void ILogEventSink.Emit(LogEvent logEvent)
        {
        }
    }
}
