namespace Lucien.HttpApi.Host.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _logDirectory;

        public FileLoggerProvider(string logDirectory)
        {
            _logDirectory = logDirectory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logFilePath = $"{_logDirectory}/app_{DateTime.UtcNow:yyyy-MM-dd}.log";
            return new FileLogger(logFilePath);
        }

        public void Dispose() { }
    }
}
