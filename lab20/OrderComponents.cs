using System;

namespace Lab20
{
    public interface IOrderValidator { bool IsValid(Order order); }
    public interface IOrderRepository { void Save(Order order); }
    public interface IEmailService { void SendOrderConfirmation(Order order); }

    //класи для окремих завданнь

    public class OrderValidator : IOrderValidator
    {
        public bool IsValid(Order order) => order.TotalAmount > 0;
    }

    public class InMemoryOrderRepository : IOrderRepository
    {
        public void Save(Order order) => 
            Console.WriteLine($"Замовлення №{order.Id} збережено в пам'ять.");
    }

    public class ConsoleEmailService : IEmailService
    {
        public void SendOrderConfirmation(Order order) => 
            Console.WriteLine($"Повідомлення надіслано клієнту {order.CustomerName}.");
    }
}