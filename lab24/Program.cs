using System;
using System.Collections.Generic;

namespace lab24
{
    // PATTERN: STRATEGY

    public interface INumericOperationStrategy
    {
        string Name { get; }
        double Execute(double value);
    }

    public class SquareOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Square";
        public double Execute(double value) => value * value;
    }

    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Cube";
        public double Execute(double value) => value * value * value;
    }

    public class SquareRootOperationStrategy : INumericOperationStrategy
    {
        public string Name => "Square Root";
        public double Execute(double value) => Math.Sqrt(value);
    }

    public class NumericProcessor
    {
        private INumericOperationStrategy _strategy;

        public NumericProcessor(INumericOperationStrategy strategy) => _strategy = strategy;

        public void SetStrategy(INumericOperationStrategy strategy) => _strategy = strategy;

        public double Process(double input) => _strategy.Execute(input);
        
        public string GetCurrentStrategyName() => _strategy.Name;
    }

    // PATTERN: OBSERVER (через Events)

    public class ResultPublisher
    {
        public event Action<double, string>? ResultCalculated;

        public void PublishResult(double result, string operationName)
        {
            ResultCalculated?.Invoke(result, operationName);
        }
    }

    //Спостерігачі

    public class ConsoleLoggerObserver
    {
        public void OnResultCalculated(double result, string op) =>
            Console.WriteLine($"[Console Log] Operation: {op}, Result: {result}");
    }

    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new List<string>();
        public void OnResultCalculated(double result, string op) =>
            History.Add($"{DateTime.Now:T}: {op} -> {result}");
    }

    public class ThresholdNotifierObserver
    {
        private readonly double _threshold;
        public ThresholdNotifierObserver(double threshold) => _threshold = threshold;

        public void OnResultCalculated(double result, string op)
        {
            if (result > _threshold)
                Console.WriteLine($"[ALERT] Threshold exceeded! {result} > {_threshold}");
        }
    }

    // ДЕМОНСТРАЦІЯ (Main)

    class Program
    {
        static void Main(string[] args)
        {
            // 1. Ініціалізація патернів
            var publisher = new ResultPublisher();
            var processor = new NumericProcessor(new SquareOperationStrategy());

            // 2. Створення та підписка спостерігачів
            var consoleObs = new ConsoleLoggerObserver();
            var historyObs = new HistoryLoggerObserver();
            var alertObs = new ThresholdNotifierObserver(100.0);

            publisher.ResultCalculated += consoleObs.OnResultCalculated;
            publisher.ResultCalculated += historyObs.OnResultCalculated;
            publisher.ResultCalculated += alertObs.OnResultCalculated;

            // 3. Робота програми
            double[] inputs = { 5, 12, 144 };

            foreach (var num in inputs)
            {
                Console.WriteLine($"\n--- Processing number: {num} ---");

                // Квадрат
                double res = processor.Process(num);
                publisher.PublishResult(res, processor.GetCurrentStrategyName());

                // Куб 
                processor.SetStrategy(new CubeOperationStrategy());
                res = processor.Process(num);
                publisher.PublishResult(res, processor.GetCurrentStrategyName());

                // Корінь
                processor.SetStrategy(new SquareRootOperationStrategy());
                res = processor.Process(num);
                publisher.PublishResult(res, processor.GetCurrentStrategyName());
            }

            // Вивід історії
            Console.WriteLine("\n--- Full History Log ---");
            historyObs.History.ForEach(Console.WriteLine);

            Console.ReadLine();
        }
    }
}