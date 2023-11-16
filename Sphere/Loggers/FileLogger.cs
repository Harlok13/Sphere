namespace Sphere.Loggers;

public class FileLogger : ILogger, IDisposable
{
    private readonly string _filePath;
    private static readonly object Lock = new();

    public FileLogger(string path) => _filePath = path;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => this;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        lock (Lock)
        {
            File.AppendAllText(_filePath, formatter(state, exception) + Environment.NewLine);
        }
    }

    public void Dispose()
    {
    }
}