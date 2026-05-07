using System;

namespace Lab20
{
    //Клас після рефакторингу
    public class OrderService
    {
        private readonly IOrderValidator _validator;
        private readonly IOrderRepository _repository;
        private readonly IEmailService _emailService;

        public OrderService(IOrderValidator validator, IOrderRepository repository, IEmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void ProcessOrder(Order order)
        {
            if (!_validator.IsValid(order))
            {
                Console.WriteLine($"Замовлення {order.Id} оформлено неправильно.");
                order.Status = OrderStatus.Cancelled;
                return;
            }

            _repository.Save(order);
            _emailService.SendOrderConfirmation(order);
            
            order.Status = OrderStatus.Processed;
            Console.WriteLine($"Замовлення {order.Id} успішно створено.");
        }
    }
}