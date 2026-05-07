using System;

public interface IDataProcessorStrategy
{
    string Process(string data);
}

public class HtmlFormatStrategy : IDataProcessorStrategy
{
    public string Process(string data) => $"<html><body>{data}</body></html>";
}

public class MarkdownFormatStrategy : IDataProcessorStrategy
{
    public string Process(string data) => $"**{data}**";
}

public class PlainTextFormatStrategy : IDataProcessorStrategy
{
    public string Process(string data) => data;
}

public class DataContext
{
    private IDataProcessorStrategy _strategy;

    public DataContext(IDataProcessorStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IDataProcessorStrategy strategy)
    {
        _strategy = strategy;
    }

    public string ExecuteProcessing(string data)
    {
        return _strategy?.Process(data);
    }
}

public class DataPublisher
{
    public event Action<string> DataProcessed;

    public void PublishDataProcessed(string data)
    {
        DataProcessed?.Invoke(data);
    }
}

public class ConsoleOutputObserver
{
    public void HandleData(string data)
    {
        Console.WriteLine($"ConsoleOutputObserver: {data}");
    }
}

public class FileSaverObserver
{
    public void HandleData(string data)
    {
        Console.WriteLine($"FileSaverObserver: {data} saved to file.");
    }
}

class Program20
{
    static void Main()
    {
        var publisher = new DataPublisher();
        var consoleObserver = new ConsoleOutputObserver();
        var fileObserver = new FileSaverObserver();

        publisher.DataProcessed += consoleObserver.HandleData;
        publisher.DataProcessed += fileObserver.HandleData;

        var context = new DataContext(new HtmlFormatStrategy());
        string result1 = context.ExecuteProcessing("Report Data 1");
        publisher.PublishDataProcessed(result1);

        context.SetStrategy(new MarkdownFormatStrategy());
        string result2 = context.ExecuteProcessing("Report Data 2");
        publisher.PublishDataProcessed(result2);
    }
}