using System;

namespace Lab20
{
    // Клас "ДО" рефакторингу
    public class InitialOrderProcessor
    {
        public void ProcessOrder(Order order)
        {
            if (order.TotalAmount <= 0)
            {
                Console.WriteLine($"Error: Замовлення {order.Id} має неправильну кількість.");
                return;
            }

            Console.WriteLine($"Збереження замовлення {order.Id} до БД");
            Console.WriteLine($"Надсилання емейлу до {order.CustomerName}");

            order.Status = OrderStatus.Processed;
            Console.WriteLine($"Замовлення {order.Id} оброблено.");
        }
    }
}