using Polly;
using Polly.Timeout;
using System;
using System.Threading;

namespace IndependentWork11
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Самостійна робота №11: Кейси Polly\n");

            //1. Виклик зовнішнього API, який може тимчасово бути недоступним.(Retry Policy)            
            Console.WriteLine("Сценарій 1: Виклик недоступного API");

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retryCount: 3, 
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 2с, 4с, 8с
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"[LOG] Помилка: {exception.Message}. Спроба #{retryCount}. Чекаємо {timeSpan.TotalSeconds}с...");
                    });

            try
            {
                retryPolicy.Execute(() => SimulateUnstableApi());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Всі спроби вичерпано. Операція провалена: {ex.Message}");
            }
                        Console.WriteLine("\n");

            //2. Доступ до бази даних, яка може мати тимчасові проблеми з підключенням(Fallback Policy)            
            Console.WriteLine("Сценарій 2: Проблеми з підключенням до БД");

          var fallbackPolicy = Policy<string>
                .Handle<Exception>()
                .Fallback(
                    fallbackValue: "Default_User_Settings", 
                    onFallback: (outcome, context) =>
                    {
                        Console.WriteLine($"[LOG] Увага! БД недоступна: {outcome.Exception?.Message}. Використовуємо запасне значення.");
                    });

            string settings = fallbackPolicy.Execute(() => SimulateBrokenDatabase());
            Console.WriteLine($"[RESULT] Поточні налаштування: {settings}");

            Console.WriteLine("\n");

            //3.Занадто довга операція (Timeout Policy)
            Console.WriteLine("Сценарій 3: Занадто довга операція");

            var timeoutPolicy = Policy
                .Timeout(TimeSpan.FromSeconds(3), TimeoutStrategy.Pessimistic, onTimeout: (context, timespan, task) => 
                {
                    Console.WriteLine($"[LOG] Операція перевищила ліміт часу ({timespan.TotalSeconds}с). Скасування...");
                });

            try
            {
                timeoutPolicy.Execute(() => SimulateHeavyProcessing());
            }
            catch (TimeoutRejectedException)
            {
                Console.WriteLine("[ERROR] Операцію скасовано через Timeout.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Інша помилка: {ex.Message}");
            }
            Console.ReadKey();
        }

        //Методи імітації
        private static int _apiCallAttempts = 0;

        //Імітація API який може бути тимчасово не доступним
        static void SimulateUnstableApi()
        {
            _apiCallAttempts++;
            Console.WriteLine($"[System] Спроба виклику API #{_apiCallAttempts}...");

            if (_apiCallAttempts <= 2)
            {
                throw new Exception("503 Service Unavailable");
            }
            Console.WriteLine("[System] API відповів успішно: 200 OK.");
        }

        //Імітація нестабільної БД
        static string SimulateBrokenDatabase()
        {
            Console.WriteLine("[System] Спроба підключення до БД...");
            throw new Exception("Connection Timeout");
        }

        //Імітація процеса, що триває довше ніж дозволено (5с при ліміті 3с)
        static void SimulateHeavyProcessing()
        {
            Console.WriteLine("[System] Початок важкої обробки даних (план: 5 сек)...");
            Thread.Sleep(5000);
            Console.WriteLine("[System] Обробка завершена (ми цього не побачимо через Timeout).");
        }
    }
}