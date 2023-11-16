namespace Sphere.Loggers;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLoggerProvider(string path) => _filePath = path;
    
    public void Dispose() { }

    public ILogger CreateLogger(string categoryName) => new FileLogger(_filePath);
}