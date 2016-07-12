using System;
using DotLiquid;

namespace Wham
{
    public enum TraceElevation
    {
        Debug,
        Info,
        Warn,
        Error,
    }

    public interface ITracer : ILiquidizable
    {
        void Trace (TraceElevation elevation, string code, string message, Exception x = null);
    }

    public static class Tracer
    {
        public const string TracerContextKey = "WhamEngine_Tracer";
        public const string TracerContextKey_SessionId = "WhamEngine_Tracer_SessionID";

        public static Func<Guid?, ITracer> GetCreateTracer = (sessionId) => {
            return new ConsoleTracer (sessionId ?? Guid.NewGuid ());
        };

        public static ITracer GetTracer (this DotLiquid.Context context, Guid? sessionId = null)
        {
            sessionId = sessionId ?? Guid.NewGuid ();

            // get the tracer from context or create a new one via callback or use a default console tracer
            ITracer tracer = (context [TracerContextKey] as ITracer)
                ?? Tracer.GetCreateTracer?.Invoke (sessionId.Value)
                ?? new ConsoleTracer (sessionId.Value);

            if (context [TracerContextKey] != tracer) {
                context [TracerContextKey_SessionId] = sessionId.ToString ();
                context [TracerContextKey] = tracer;
            }

            return tracer;
        }

        public static void Debug (this ITracer tracer, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Debug, null, message, x);
        }

        public static void Debug (this ITracer tracer, string code, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Debug, code, message, x);
        }

        public static void Info (this ITracer tracer, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Info, null, message, x);
        }

        public static void Info (this ITracer tracer, string code, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Info, code, message, x);
        }

        public static void Warn (this ITracer tracer, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Warn, null, message, x);
        }

        public static void Warn (this ITracer tracer, string code, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Warn, code, message, x);
        }

        public static void Error (this ITracer tracer, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Error, null, message, x);
        }

        public static void Error (this ITracer tracer, string code, string message, Exception x = null)
        {
            tracer.Trace (TraceElevation.Error, code, message, x);
        }

        internal class ConsoleTracer : ITracer
        {
            protected readonly Guid SessionId;
            public ConsoleTracer (Guid sessionId)
            {
                SessionId = sessionId;
                this.Info ("Session starting");
            }

            public object ToLiquid ()
            {
                return this;
            }

            public void Trace (TraceElevation elevation, string code, string message, Exception x = null)
            {
                Console.WriteLine ("{0} {1}{2}{3} [{6}]{4}{5}",
                                                  elevation,
                                                  code,
                                                  string.IsNullOrEmpty (code) ? null : " ",
                                                  message,
                                                  x == null ? null : "\r\n Exception: ",
                                                  x,
                                                  SessionId);
            }
        }
    }

}

