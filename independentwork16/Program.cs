using System;

namespace IndependentWork16
{
    //порушення SRP
    public class BadPaymentProcessor
    {
        public void ProcessPayment(double amount, string cardNumber)
        {
            // 1. Валідація
            if (string.IsNullOrEmpty(cardNumber) || amount <= 0)
            {
                Console.WriteLine("Помилка: Невірні дані платежу.");
                return;
            }

            // 2. Списання коштів
            Console.WriteLine($"Списання {amount} грн з картки {cardNumber}...");

            // 3. Логування
            Console.WriteLine($"[LOG]: Транзакція на суму {amount} успішна. Дата: {DateTime.Now}");

            // 4. Відправка SMS
            Console.WriteLine($"[SMS]: Платіж на {amount} грн виконано успішно.");
        }
    }

    //рефакторинг

    //інтерфейси
    public interface IPaymentValidator { bool Validate(double amount, string cardNumber); }
    public interface IPaymentGateway { bool Charge(double amount, string cardNumber); }
    public interface ITransactionLogger { void Log(string message); }
    public interface ISmsService { void SendSms(string message); }

    public class PaymentValidator : IPaymentValidator
    {
        public bool Validate(double amount, string cardNumber) => !string.IsNullOrEmpty(cardNumber) && amount > 0;
    }

    public class PaymentGateway : IPaymentGateway
    {
        public bool Charge(double amount, string cardNumber)
        {
            Console.WriteLine($" Успішно списано {amount} грн.");
            return true;
        }
    }

    public class TransactionLogger : ITransactionLogger
    {
        public void Log(string message) => Console.WriteLine($"{DateTime.Now}: {message}");
    }

    public class SmsService : ISmsService
    {
        public void SendSms(string message) => Console.WriteLine($"Надсилання повідомлення: {message}");
    }

    //основний сервіс 
    public class PaymentService
    {
        private readonly IPaymentValidator _validator;
        private readonly IPaymentGateway _gateway;
        private readonly ITransactionLogger _logger;
        private readonly ISmsService _smsService;

        public PaymentService(IPaymentValidator validator, IPaymentGateway gateway, ITransactionLogger logger, ISmsService smsService)
        {
            _validator = validator;
            _gateway = gateway;
            _logger = logger;
            _smsService = smsService;
        }

        public void ExecutePayment(double amount, string cardNumber)
        {
            if (!_validator.Validate(amount, cardNumber))
            {
                _logger.Log("Спроба платежу з некоректними даними.");
                return;
            }

            if (_gateway.Charge(amount, cardNumber))
            {
                _logger.Log($"Платіж {amount} грн проведено.");
                _smsService.SendSms($"Ваш рахунок поповнено на {amount} грн.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Демонстрація SRP (Payment System)\n");

            // Налаштування компонентів
            var validator = new PaymentValidator();
            var gateway = new PaymentGateway();
            var logger = new TransactionLogger();
            var sms = new SmsService();

            // Створення сервісу
            var paymentService = new PaymentService(validator, gateway, logger, sms);

            // Виконання операції
            paymentService.ExecutePayment(250.50, "4444-5555-6666-7777");

            Console.WriteLine("\nРобота завершена успішно.");
        }
    }
}