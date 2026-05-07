using System;

namespace Lab20
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var validOrder = new Order(1, "Артем", 500.00m);
            var invalidOrder = new Order(2, "Макс", -15.0m);

            Console.WriteLine("Варіант до рефакторингу");
            var oldProcessor = new InitialOrderProcessor();
            oldProcessor.ProcessOrder(validOrder);

            Console.WriteLine("\nВаріант після рефакторингу (SRP)");
                var orderService = new OrderService(
                new OrderValidator(),
                new InMemoryOrderRepository(),
                new ConsoleEmailService()
            );

            Console.WriteLine("Обробка валідного:");
            orderService.ProcessOrder(validOrder);

            Console.WriteLine("\nОбробка невалідного:");
            orderService.ProcessOrder(invalidOrder);

            Console.ReadKey();
        }
    }
}