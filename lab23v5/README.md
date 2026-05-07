Звіт з лабораторної роботи №23: ISP та DIP

Тема: Рефакторинг ієрархії класів та впровадження Dependency Injection (DI).

Мета: Усунути жорстку залежність модулів та розділити інтерфейси для покращення гнучкості системи.

1. Аналіз коду до рефакторингу

У початковій реалізації клас `UserAccountManager` виглядав приблизно так:

// Поганий приклад
public class UserAccountManager {
    private SqlDatabase _db = new SqlDatabase(); // Жорстка залежність (DIP violation)
    private SmtpClient _email = new SmtpClient(); // Жорстка залежність

    public void Register(string user) {
        _db.Save(user);
        _email.Send("Welcome!");
    }
}

Виявлені порушення:

1. Порушення DIP (Dependency Inversion Principle): Клас високого рівня (`UserAccountManager`) залежить від конкретних класів низького рівня (`SqlDatabase`). Будь-яка зміна в базі даних вимагає перекомпіляції менеджера.
2. Порушення ISP (Interface Segregation Principle): Якщо створити один інтерфейс `IMessenger` з методами `SendEmail()` та `SendSMS()`, то сервіс, який вміє відправляти лише пошту, буде змушений реалізовувати непотрібний йому метод SMS.
3. Низька тестованість: Ми не можемо протестувати `Register`, не записуючи дані в реальну базу та не відправляючи реальні листи.

2. Реалізація рефакторингу

Крок 1: Виділення інтерфейсів (ISP)

Ми розділили функціонал повідомлень на два вузьких інтерфейси.

public interface IDatabase { void Save(string data); }
public interface IEmailService { void SendEmail(string to, string body); }
public interface ISmsService { void SendSms(string phone, string message); }

Крок 2: Впровадження інверсії залежностей (DIP)

Клас `UserAccountManager` тепер приймає інтерфейси через конструктор.

public class UserAccountManager {
    private readonly IDatabase _database;
    private readonly IEmailService _email;
    private readonly ISmsService _sms;

    // Впровадження залежностей (Constructor Injection)
    public UserAccountManager(IDatabase db, IEmailService email, ISmsService sms) {
        _database = db;
        _email = email;
        _sms = sms;
    }

    public void Register(string name, string email, string phone) {
        _database.Save(name);
        _email.SendEmail(email, "Привіт!");
        _sms.SendSms(phone, "Код: 1234");
    }
}

3. Демонстрація роботи в Main

static void Main() {
    // Конфігуруємо залежності зовні
    IDatabase db = new SqlDatabase();
    IEmailService email = new SmtpClientService();
    ISmsService sms = new SmsGatewayService();

    // Передаємо їх у менеджер
    var manager = new UserAccountManager(db, email, sms);
    manager.Register("Ivan_Petrov", "ivan@mail.com", "+380501234567");
}

4. Висновок

В ході виконання роботи було доведено, що використання принципів ISP та DIP значно покращує архітектуру ПЗ:

1. Розв’язаність (Decoupling): Головний клас перестав залежати від деталей реалізації. Тепер ми можемо змінити `SqlDatabase` на `MongoDatabase`, не змінюючи жодного рядка в `UserAccountManager`.
2. Чистота інтерфейсів: Завдяки ISP, класи реалізують лише той функціонал, який їм дійсно потрібен. Це робить код зрозумілішим і запобігає появі "пустих" методів-заглушок.
3. Полегшення тестування: Використання DI через конструктор дозволяє підміняти реальні сервіси на Mock-об’єкти в Unit-тестах, що забезпечує швидку та ізольовану перевірку логіки.
4. Масштабованість: Система стала відкритою для розширення (можна легко додати нові типи повідомлень), але закритою для модифікації існуючого перевіреного коду.