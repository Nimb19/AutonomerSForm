using System;
using System.Threading;
using System.Windows.Forms;

namespace AutonomerSForm
{
    public class TextBoxLogger : AbstractLogger
    {
        public TextBox TextBox { get; }
        public SynchronizationContext SyncContext { get; }

        public TextBoxLogger(TextBox textBox, SynchronizationContext syncContext)
        {
            TextBox = textBox;
            SyncContext = syncContext;
        }

        protected override void PrivateWrite(string fullMsg)
        {
            SyncContext.Post(delegate
            {
                TextBox.Text += fullMsg + Environment.NewLine;
                TextBox.SelectionStart = TextBox.Text.Length;
                TextBox.ScrollToCaret();
            }, null);
        }
    }

    public abstract class AbstractLogger : ILogger
    {
        public virtual LogLevel LogLevel { get; set; } = LogLevel.Trace;

        protected abstract void PrivateWrite(string fullMsg);

        public virtual void Write(LogLevel logLevel, string msg)
        {
            if (LogLevel >= logLevel)
            {
                var logLevelToString = string.Empty;
                switch (logLevel)
                {
                    case LogLevel.Trace: logLevelToString = "TRC"; break;
                    case LogLevel.Info: logLevelToString = "INF"; break;
                    case LogLevel.Error: logLevelToString = "ERR"; break;
                    case LogLevel.Warning: logLevelToString = "WRN"; break;
                    case LogLevel.Debug: logLevelToString = "DBG"; break;
                }

                var fullMasg = $"{DateTime.Now} [{logLevelToString}] {msg}";
                PrivateWrite(fullMasg);
            }
        }

        public virtual void Error(Exception exception)
        {
            Write(LogLevel.Error, $"{exception.Message} ({exception.InnerException}).{Environment.NewLine}{exception.StackTrace}");
        }

        public override string ToString()
        {
            return $"Logger={this.GetType().Name}: LogLevel={LogLevel}";
        }
    }

    public interface ILogger
    {
        LogLevel LogLevel { get; set; }
        void Write(LogLevel logLevel, string msg);
        void Error(Exception exception);
    }

    public enum LogLevel
    {
        Error = 0,
        Warning = 1,
        Info = 2,
        Debug = 3,
        Trace = 4,
    }

    public static class LoggerExtensions
    {
        public static ILogger Error(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Error, msg);
            return logger;
        }

        public static ILogger Warning(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Warning, msg);
            return logger;
        }

        public static ILogger WriteLine(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Info, msg);
            return logger;
        }

        public static ILogger Debug(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Debug, msg);
            return logger;
        }

        public static ILogger Trace(this ILogger logger, string msg)
        {
            logger.Write(LogLevel.Trace, msg);
            return logger;
        }
    }
}
