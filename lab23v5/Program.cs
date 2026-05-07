using System;

namespace lab23
{
    // ЧАСТИНА 1: ІНТЕРФЕЙСИ (ISP - Interface Segregation)

    public interface IDatabase { void Save(string data); }
    public interface IEmailService { void SendEmail(string to, string body); }
    public interface ISmsService { void SendSms(string phone, string message); }

    // ЧАСТИНА 2: РЕАЛІЗАЦІЇ 

    public class SqlDatabase : IDatabase
    {
        public void Save(string data) => Console.WriteLine($"[DB] User '{data}' saved to SQL.");
    }

    public class SmtpClientService : IEmailService
    {
        public void SendEmail(string to, string body) => 
            Console.WriteLine($"[Email] Sent to {to}: {body}");
    }

    public class SmsGatewayService : ISmsService
    {
        public void SendSms(string phone, string message) => 
            Console.WriteLine($"[SMS] Sent to {phone}: {message}");
    }

    // ЧАСТИНА 3: РЕФАКТОРИНГ (DIP - Dependency Injection)

    public class UserAccountManager
    {
        private readonly IDatabase _database;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public UserAccountManager(IDatabase database, IEmailService email, ISmsService sms)
        {
            _database = database;
            _emailService = email;
            _smsService = sms;
        }

        public void Register(string username, string email, string phone)
        {
            _database.Save(username);
            _emailService.SendEmail(email, "Welcome to our platform!");
            _smsService.SendSms(phone, "Your account is active.");
            
            Console.WriteLine($"User {username} registered successfully!");
        }
    }

    // ЧАСТИНА 4: ДЕМОНСТРАЦІЯ (Main)

    class Program
    {
        static void Main(string[] args)
        {
            // 1. Конфігурація залежностей (Composition Root)
            IDatabase db = new SqlDatabase();
            IEmailService email = new SmtpClientService();
            ISmsService sms = new SmsGatewayService();

            // 2. Впровадження залежностей у головний клас
            var manager = new UserAccountManager(db, email, sms);

            // 3. Перевірка роботи
            Console.WriteLine("Starting registration process...");
            manager.Register("Alex_V", "alex@example.com", "+380991234567");

            Console.WriteLine("\nLab 23 completed successfully.");
            Console.ReadLine();
        }
    }
}