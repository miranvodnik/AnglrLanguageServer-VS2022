using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace AnglrLogLibrary
{
    public enum AnglrLogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        None = 5
    }

    public interface IAnglrLogger
    {
        AnglrLogLevel MinimumLevel { get; set; }

        void Log (AnglrLogLevel level, string message, uint flags = 0);
        void Log (AnglrLogLevel level, Exception ex, string message, uint flags = 0);
        void Log<T> (AnglrLogLevel level, Func<T, string> f, T data, uint flags = 0);

        // Convenience helpers
        void Trace (string message, uint flags = 0);
        void Trace (Exception ex, string message, uint flags = 0);
        void Trace<T> (Func<T, string> f, T data, uint flags = 0);
        void TraceRaw (string message, uint flags = 0);
        void TraceRaw (Exception ex, string message, uint flags = 0);
        void TraceRaw<T> (Func<T, string> f, T data, uint flags = 0);
        void TraceLine (string message, uint flags = 0);
        void TraceLine (Exception ex, string message, uint flags = 0);
        void TraceLine<T> (Func<T, string> f, T data, uint flags = 0);
        void TraceRawLine (string message, uint flags = 0);
        void TraceRawLine (Exception ex, string message, uint flags = 0);
        void TraceRawLine<T> (Func<T, string> f, T data, uint flags = 0);
        void Debug (string message, uint flags = 0);
        void Debug (Exception ex, string message, uint flags = 0);
        void Debug<T> (Func<T, string> f, T data, uint flags = 0);
        void DebugRaw (string message, uint flags = 0);
        void DebugRaw (Exception ex, string message, uint flags = 0);
        void DebugRaw<T> (Func<T, string> f, T data, uint flags = 0);
        void DebugLine (string message, uint flags = 0);
        void DebugLine (Exception ex, string message, uint flags = 0);
        void DebugLine<T> (Func<T, string> f, T data, uint flags = 0);
        void DebugRawLine (string message, uint flags = 0);
        void DebugRawLine (Exception ex, string message, uint flags = 0);
        void DebugRawLine<T> (Func<T, string> f, T data, uint flags = 0);
        void Info (string message, uint flags = 0);
        void Info (Exception ex, string message, uint flags = 0);
        void Info<T> (Func<T, string> f, T data, uint flags = 0);
        void InfoRaw (string message, uint flags = 0);
        void InfoRaw (Exception ex, string message, uint flags = 0);
        void InfoRaw<T> (Func<T, string> f, T data, uint flags = 0);
        void InfoLine (string message, uint flags = 0);
        void InfoLine (Exception ex, string message, uint flags = 0);
        void InfoLine<T> (Func<T, string> f, T data, uint flags = 0);
        void InfoRawLine (string message, uint flags = 0);
        void InfoRawLine (Exception ex, string message, uint flags = 0);
        void InfoRawLine<T> (Func<T, string> f, T data, uint flags = 0);
        void Warn (string message, uint flags = 0);
        void Warn (Exception ex, string message, uint flags = 0);
        void Warn<T> (Func<T, string> f, T data, uint flags = 0);
        void WarnRaw (string message, uint flags = 0);
        void WarnRaw (Exception ex, string message, uint flags = 0);
        void WarnRaw<T> (Func<T, string> f, T data, uint flags = 0);
        void WarnLine (string message, uint flags = 0);
        void WarnLine (Exception ex, string message, uint flags = 0);
        void WarnLine<T> (Func<T, string> f, T data, uint flags = 0);
        void WarnRawLine (string message, uint flags = 0);
        void WarnRawLine (Exception ex, string message, uint flags = 0);
        void WarnRawLine<T> (Func<T, string> f, T data, uint flags = 0);
        void Error (string message, uint flags = 0);
        void Error (Exception ex, string message, uint flags = 0);
        void Error<T> (Func<T, string> f, T data, uint flags = 0);
        void ErrorRaw (string message, uint flags = 0);
        void ErrorRaw (Exception ex, string message, uint flags = 0);
        void ErrorRaw<T> (Func<T, string> f, T data, uint flags = 0);
        void ErrorLine (string message, uint flags = 0);
        void ErrorLine (Exception ex, string message, uint flags = 0);
        void ErrorLine<T> (Func<T, string> f, T data, uint flags = 0);
        void ErrorRawLine (string message, uint flags = 0);
        void ErrorRawLine (Exception ex, string message, uint flags = 0);
        void ErrorRawLine<T> (Func<T, string> f, T data, uint flags = 0);
    }

    public abstract class AnglrLoggerBase : IAnglrLogger
    {
        public const uint ForceWriteLine = (1 << 0);
        public const uint WriteOnlyMsg = (1 << 1);
        public const uint AllAnglrLoggerFlags = ForceWriteLine | WriteOnlyMsg;

        public AnglrLogLevel MinimumLevel { get; set; } = AnglrLogLevel.Info;

        public void Log (AnglrLogLevel level, string message, uint flags = ForceWriteLine)
        {
            if (level < MinimumLevel)
                return;

            if ((flags & ForceWriteLine) != 0)
                WriteLine (level, message, flags);
            else
                Write (level, message, flags);
        }

        public void Log (AnglrLogLevel level, Exception ex, string message, uint flags = ForceWriteLine)
        {
            if (level < MinimumLevel)
                return;

            if ((flags & ForceWriteLine) != 0)
                WriteLine (level, $"{message}\n{ex}", flags);
            else
                Write (level, $"{message}\n{ex}", flags);
        }

        public void Log<T> (AnglrLogLevel level, Func<T, string> f, T data, uint flags = ForceWriteLine)
        {
            if (level < MinimumLevel)
                return;

            if ((flags & ForceWriteLine) != 0)
                WriteLine (level, f (data), flags);
            else
                Write (level, f (data), flags);
        }

        protected abstract void Write (AnglrLogLevel level, string message, uint flags = 0);
        protected abstract void WriteLine (AnglrLogLevel level, string message, uint flags = 0);

        public void Trace (string m, uint flags = 0) => Log (AnglrLogLevel.Trace, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Trace (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Trace, ex, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Trace<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Trace, f, data, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void TraceRaw (string m, uint flags = 0) => Log (AnglrLogLevel.Trace, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void TraceRaw (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Trace, ex, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void TraceRaw<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Trace, f, data, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void TraceLine (string m, uint flags = 0) => Log (AnglrLogLevel.Trace, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void TraceLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Trace, ex, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void TraceLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Trace, f, data, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void TraceRawLine (string m, uint flags = 0) => Log (AnglrLogLevel.Trace, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void TraceRawLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Trace, ex, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void TraceRawLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Trace, f, data, flags | ForceWriteLine | WriteOnlyMsg);
        public void Debug (string m, uint flags = 0) => Log (AnglrLogLevel.Debug, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Debug (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Debug, ex, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Debug<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Debug, f, data, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void DebugRaw (string m, uint flags = 0) => Log (AnglrLogLevel.Debug, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void DebugRaw (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Debug, ex, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void DebugRaw<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Debug, f, data, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void DebugLine (string m, uint flags = 0) => Log (AnglrLogLevel.Debug, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void DebugLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Debug, ex, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void DebugLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Debug, f, data, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void DebugRawLine (string m, uint flags = 0) => Log (AnglrLogLevel.Debug, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void DebugRawLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Debug, ex, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void DebugRawLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Debug, f, data, flags | ForceWriteLine | WriteOnlyMsg);
        public void Info (string m, uint flags = 0) => Log (AnglrLogLevel.Info, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Info (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Info, ex, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Info<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Info, f, data, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void InfoRaw (string m, uint flags = 0) => Log (AnglrLogLevel.Info, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void InfoRaw (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Info, ex, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void InfoRaw<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Info, f, data, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void InfoLine (string m, uint flags = 0) => Log (AnglrLogLevel.Info, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void InfoLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Info, ex, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void InfoLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Info, f, data, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void InfoRawLine (string m, uint flags = 0) => Log (AnglrLogLevel.Info, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void InfoRawLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Info, ex, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void InfoRawLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Info, f, data, flags | ForceWriteLine | WriteOnlyMsg);
        public void Warn (string m, uint flags = 0) => Log (AnglrLogLevel.Warn, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Warn (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Warn, ex, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Warn<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Warn, f, data, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void WarnRaw (string m, uint flags = 0) => Log (AnglrLogLevel.Warn, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void WarnRaw (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Warn, ex, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void WarnRaw<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Warn, f, data, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void WarnLine (string m, uint flags = 0) => Log (AnglrLogLevel.Warn, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void WarnLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Warn, ex, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void WarnLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Warn, f, data, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void WarnRawLine (string m, uint flags = 0) => Log (AnglrLogLevel.Warn, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void WarnRawLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Warn, ex, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void WarnRawLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Warn, f, data, flags | ForceWriteLine | WriteOnlyMsg);
        public void Error (string m, uint flags = 0) => Log (AnglrLogLevel.Error, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Error (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Error, ex, m, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void Error<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Error, f, data, flags & ~(ForceWriteLine | WriteOnlyMsg));
        public void ErrorRaw (string m, uint flags = 0) => Log (AnglrLogLevel.Error, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void ErrorRaw (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Error, ex, m, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void ErrorRaw<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Error, f, data, (flags & ~ForceWriteLine) | WriteOnlyMsg);
        public void ErrorLine (string m, uint flags = 0) => Log (AnglrLogLevel.Error, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void ErrorLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Error, ex, m, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void ErrorLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Error, f, data, (flags & ~WriteOnlyMsg) | ForceWriteLine);
        public void ErrorRawLine (string m, uint flags = 0) => Log (AnglrLogLevel.Error, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void ErrorRawLine (Exception ex, string m, uint flags = 0) => Log (AnglrLogLevel.Error, ex, m, flags | ForceWriteLine | WriteOnlyMsg);
        public void ErrorRawLine<T> (Func<T, string> f, T data, uint flags = 0) => Log (AnglrLogLevel.Error, f, data, flags | ForceWriteLine | WriteOnlyMsg);
    }

    public class ConsoleAnglrLogger : AnglrLoggerBase
    {
        protected override void Write (AnglrLogLevel level, string message, uint flags = 0)
        {
            if (level >= AnglrLogLevel.Error)
                if ((flags & WriteOnlyMsg) != 0)
                    Console.Error.Write (message);
                else
                    Console.Error.Write ($"{DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff")} - {level} - {message}");
            else if ((flags & WriteOnlyMsg) != 0)
                Console.Write (message);
            else
                Console.Write ($"{DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff")} - {level} - {message}");
        }
        protected override void WriteLine (AnglrLogLevel level, string message, uint flags = 0)
        {
            if (level >= AnglrLogLevel.Error)
                if ((flags & WriteOnlyMsg) != 0)
                    Console.Error.WriteLine (message);
                else
                    Console.Error.WriteLine ($"{DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff")} - {level} - {message}");
            else if ((flags & WriteOnlyMsg) != 0)
                Console.WriteLine (message);
            else
                Console.WriteLine ($"{DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.fff")} - {level} - {message}");
        }
    }

    public class VoidAnglrLogger : AnglrLoggerBase
    {
        protected override void Write (AnglrLogLevel level, string message, uint flags = 0) { }
        protected override void WriteLine (AnglrLogLevel level, string message, uint flags = 0) { }
    }
}
