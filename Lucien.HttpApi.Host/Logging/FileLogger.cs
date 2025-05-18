using System.Text;

namespace Lucien.HttpApi.Host.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private static readonly object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }


        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Error;


        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var logRecord = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {logLevel} | {formatter(state, exception)}{Environment.NewLine}";

            lock (_lock)
            {
                File.AppendAllText(_filePath, logRecord, Encoding.UTF8);
            }
        }
    }
}
