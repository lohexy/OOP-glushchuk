using System;
using System.Collections.Generic;
using System.IO;

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[Console Log]: {message}");
}

public class FileLogger : ILogger
{
    private readonly string _path = "log.txt";
    public void Log(string message) => File.AppendAllText(_path, $"[File Log]: {message}{Environment.NewLine}");
}

public abstract class LoggerFactory
{
    public abstract ILogger CreateLogger();
}

public class ConsoleLoggerFactory : LoggerFactory
{
    public override ILogger CreateLogger() => new ConsoleLogger();
}

public class FileLoggerFactory : LoggerFactory
{
    public override ILogger CreateLogger() => new FileLogger();
}

public class LoggerManager
{
    private static LoggerManager _instance;
    private LoggerFactory _factory;
    private ILogger _currentLogger;

    private LoggerManager() { }

    public static LoggerManager Instance => _instance ??= new LoggerManager();

    public void SetFactory(LoggerFactory factory)
    {
        _factory = factory;
        _currentLogger = _factory.CreateLogger();
    }

    public void Log(string message) => _currentLogger?.Log(message);
}

public interface IDataProcessorStrategy
{
    string Process(string data);
}

public class EncryptDataStrategy : IDataProcessorStrategy
{
    public string Process(string data) => $"Encrypted({data})";
}

public class CompressDataStrategy : IDataProcessorStrategy
{
    public string Process(string data) => $"Compressed({data})";
}

public class DataContext
{
    private IDataProcessorStrategy _strategy;

    public DataContext(IDataProcessorStrategy strategy) => _strategy = strategy;

    public void SetStrategy(IDataProcessorStrategy strategy) => _strategy = strategy;

    public string ProcessData(string data) => _strategy.Process(data);
}

public class DataProcessedEventArgs : EventArgs
{
    public string Data { get; }
    public DataProcessedEventArgs(string data) => Data = data;
}

public class DataPublisher
{
    public event EventHandler<DataProcessedEventArgs> DataProcessed;

    public void PublishDataProcessed(string data)
    {
        DataProcessed?.Invoke(this, new DataProcessedEventArgs(data));
    }
}

public class ProcessingLoggerObserver
{
    public void OnDataProcessed(object sender, DataProcessedEventArgs e)
    {
        LoggerManager.Instance.Log($"Observer caught processed data: {e.Data}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Сценарій 1: Повна інтеграція");
        LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory());
        
        var context = new DataContext(new EncryptDataStrategy());
        var publisher = new DataPublisher();
        var observer = new ProcessingLoggerObserver();

        publisher.DataProcessed += observer.OnDataProcessed;

        string data1 = "Data_01";
        string processed1 = context.ProcessData(data1);
        publisher.PublishDataProcessed(processed1);

        Console.WriteLine("\nСценарій 2: Динамічна зміна логера");
        LoggerManager.Instance.SetFactory(new FileLoggerFactory());
        
        string data2 = "Data_02";
        string processed2 = context.ProcessData(data2);
        publisher.PublishDataProcessed(processed2);
        Console.WriteLine("Дані логуються у файл log.txt");

        Console.WriteLine("\nСценарій 3: Динамічна зміна стратегії");
        LoggerManager.Instance.SetFactory(new ConsoleLoggerFactory());
        context.SetStrategy(new CompressDataStrategy());

        string data3 = "Data_03";
        string processed3 = context.ProcessData(data3);
        publisher.PublishDataProcessed(processed3);
    }
}