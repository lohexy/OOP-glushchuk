using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace Lab7
{
    //1. Узагальнений клас RetryHelper
    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation, 
            int retryCount = 3, 
            TimeSpan? initialDelay = null, 
            Func<Exception, bool> shouldRetry = null)
        {
            //1 секунда затримки за замовчуванням
            var delay = initialDelay ?? TimeSpan.FromSeconds(1);

            for (int attempt = 0; attempt <= retryCount; attempt++)
            {
                try
                {
                    return operation();
                }
                catch (Exception ex)
                {
                    //1) Перевіряємо, чи це остання спроба
                    if (attempt == retryCount)
                    {
                        Console.WriteLine($"[ERROR] Вичерпано ліміт спроб ({retryCount}). Останній виняток: {ex.Message}");
                        throw; 
                    }

                    //2) Перевіряємо, чи підходить цей тип помилки для повтору
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        Console.WriteLine($"[ERROR] Помилка '{ex.GetType().Name}' не підлягає повторній спробі.");
                        throw;
                    }

                    //3) Обчислюємо затримку: delay * 2^attempt
                    TimeSpan currentDelay = TimeSpan.FromTicks(delay.Ticks * (long)Math.Pow(2, attempt));

                    Console.WriteLine($"[LOG] Невдача (Спроба {attempt + 1}/{retryCount}). " +
                                      $"Помилка: {ex.Message}. " +
                                      $"Повтор через {currentDelay.TotalSeconds} с...");

                    Thread.Sleep(currentDelay);
                }
            }

            throw new Exception("Непередбачувана помилка в RetryHelper");
        }
    }

    //2. Класи імітації:FileProcessor і NetworkClient
    public class FileProcessor
    {
        private int _attempts = 0;

        //Імітує оновлення конфігурації у файлі
        public void UpdateConfig(string path, string key, string value)
        {
            _attempts++;
            Console.WriteLine($"[Файл] Спроба запису у файл '{path}' (виклик №{_attempts})...");

            if (_attempts <= 2)
            {
                throw new IOException($"Файл '{path}' заблокований іншим процесом.");
            }

            Console.WriteLine("[Файл] Конфігурацію успішно оновлено.");
        }
    }

    public class NetworkClient
    {
        private int _attempts = 0;

        //Імітація відправки запиту.
        public bool SendConfigUpdate(string url, string configJson)
        {
            _attempts++;
            Console.WriteLine($"[Мережа] POST запит до '{url}' (виклик №{_attempts})...");

            if (_attempts <= 3)
            {
                throw new HttpRequestException($"Сервер повернув 503 Service Unavailable.");
            }

            Console.WriteLine("[Мережа] Дані успішно синхронізовано.");
            return true;
        }
    }

    //3.Демонстрація роботи
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Лабораторна робота №7: Retry Pattern\n");

            var fileProcessor = new FileProcessor();
            var networkClient = new NetworkClient();

            //Правило shouldRetry:повторювати тільки якщо помилка IOException АБО HttpRequestException
            Func<Exception, bool> myRetryLogic = (ex) =>
                ex is IOException || ex is HttpRequestException;

            //КРОК 1: Робота з файлом (FileProcessor)
            Console.WriteLine("Сценарій 1: Оновлення локального файлу конфігурації");

            try
            {
                RetryHelper.ExecuteWithRetry<bool>(() =>
                {
                    fileProcessor.UpdateConfig("config.json", "Theme", "Dark");
                    return true;
                },
                retryCount: 3,
                initialDelay: TimeSpan.FromSeconds(1),
                shouldRetry: myRetryLogic);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
            }

            Console.WriteLine("\n");

            //КРОК 2: Робота з мережею (NetworkClient)
            Console.WriteLine("Сценарій 2: Синхронізація з сервером");

            try
            {
                bool result = RetryHelper.ExecuteWithRetry<bool>(() =>
                    networkClient.SendConfigUpdate("https://api.example.com/config", "{ 'Theme': 'Dark' }"),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromSeconds(0.5),
                    shouldRetry: myRetryLogic
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL ERROR: {ex.Message}");
            }

            Console.WriteLine("\nРоботу завершено");
            Console.ReadKey();
        }
    }
}