using System;
using System.Globalization;
using lab21;

class Program
{
    static void Main()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

         try
{
    Console.WriteLine("СИСТЕМА ДОСТАВКИ");
    Console.Write("Тип (Standard/Express/International/Night): ");
    
    string shipType = Console.ReadLine() ?? ""; 
    
    var shipStrategy = ShippingStrategyFactory.CreateStrategy(shipType);
    var delivery = new DeliveryService();
    decimal shipCost = delivery.CalculateDeliveryCost(150, 5, shipStrategy);
    Console.WriteLine($"Вартість доставки: {shipCost} грн\n");

    Console.WriteLine("АБОНЕМЕНТ У СПОРТЗАЛ");
    Console.Write("Тип абонемента (Morning/Day/Full): ");
    
    string gymType = Console.ReadLine() ?? "";
    
    var pass = GymPassFactory.CreatePass(gymType);
    decimal gymPrice = pass.CalculatePrice(3, true); 
    Console.WriteLine($"Вартість абонемента: {gymPrice} грн");
}
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("\nРоботу завершено. Натисніть будь-яку клавішу...");
        Console.ReadKey();
    }
}