using System;
using Xunit;

public class IntegrationTests21
{
    [Fact]
    public void GraphicsEngine_SingletonInstance_IsStable()
    {
        var instance1 = GraphicsEngine.Instance;
        var instance2 = GraphicsEngine.Instance;
        
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void DataContext_ExecuteProcessing_HtmlStrategy_ReturnsHtml()
    {
        var context = new DataContext(new HtmlFormatStrategy());
        var result = context.ExecuteProcessing("Test");
        
        Assert.Equal("<html><body>Test</body></html>", result);
    }

    [Fact]
    public void DataPublisher_DataProcessedEvent_TriggersSubscribers()
    {
        var publisher = new DataPublisher();
        bool eventTriggered = false;
        
        publisher.DataProcessed += (data) => eventTriggered = true;
        publisher.PublishDataProcessed("Test");
        
        Assert.True(eventTriggered);
    }

    [Fact]
    public void DataContext_ExecuteProcessing_NullStrategy_ReturnsNull()
    {
        var context = new DataContext(null);
        var result = context.ExecuteProcessing("Test");
        
        Assert.Null(result);
    }

    [Fact]
    public void DataPublisher_NoSubscribers_DoesNotThrow()
    {
        var publisher = new DataPublisher();
        
        Action act = () => publisher.PublishDataProcessed("Test"); 
        
        var exception = Record.Exception(act);
        Assert.Null(exception);
    }
}

public class GraphicsEngine
{
    private static GraphicsEngine _instance;
    private GraphicsEngine() { }
    public static GraphicsEngine Instance => _instance ??= new GraphicsEngine();
}

public interface IDataProcessorStrategy
{
    string Process(string data);
}

public class HtmlFormatStrategy : IDataProcessorStrategy
{
    public string Process(string data) => $"<html><body>{data}</body></html>";
}

public class DataContext
{
    private IDataProcessorStrategy _strategy;

    public DataContext(IDataProcessorStrategy strategy)
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