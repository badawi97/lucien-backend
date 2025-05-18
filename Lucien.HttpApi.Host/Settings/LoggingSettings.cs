namespace Lucien.HttpApi.Host.Settings
{
    public class LoggingSettings
    {
        public LogDirectory? LogDirectory { get; set; }
    }

    public class LogDirectory
    {
        public string? FolderName { get; set; }

    }
}
