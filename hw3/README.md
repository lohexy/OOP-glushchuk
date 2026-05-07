Домашня робота №3
Принципи ISP та DIP (SOLID) — C#
1. Принцип ISP (Interface Segregation Principle)
ISP означає, що класи не повинні залежати від методів, які вони не використовують.
Краще розділяти великі інтерфейси на менші, спеціалізовані.

Приклад порушення ISP

public interface IUserRepository
{
    User GetById(int id);
    void Save(User user);
    void Delete(int id);
    IEnumerable<User> GetAll();
}


public class UserReaderService
{
    private readonly IUserRepository _repository;

    public UserReaderService(IUserRepository repository)
    {
        _repository = repository;
    }

    public User GetUser(int id)
    {
        return _repository.GetById(id);
    }
}

Проблема: UserReaderService використовує лише GetById, але змушений залежати від методів Save, Delete, GetAll, які йому не потрібні.

Вирішення проблеми (дотримання ISP)

public interface IUserReader
{
    User GetById(int id);
}

public interface IUserWriter
{
    void Save(User user);
    void Delete(int id);
}

public class UserRepository : IUserReader, IUserWriter
{
    public User GetById(int id)
    {
        return new User { Id = id, Name = "Test User" };
    }

    public void Save(User user) { }
    public void Delete(int id) { }
}

public class UserReaderService
{
    private readonly IUserReader _reader;

    public UserReaderService(IUserReader reader)
    {
        _reader = reader;
    }

    public User GetUser(int id)
    {
        return _reader.GetById(id);
    }
}

Клас залежить лише від необхідної функціональності.

 Принцип DIP (Dependency Inversion Principle)

DIP стверджує, що високорівневі модулі не повинні залежати від конкретних реалізацій, а лише від абстракцій.

Приклад без DIP

public class EmailService
{
    public void Send(string message) { }
}

public class NotificationService
{
    private readonly EmailService _emailService = new EmailService();

    public void Notify(string message)
    {
        _emailService.Send(message);
    }
}

Проблема: NotificationService напряму залежить від EmailService.

Застосування DIP через Dependency Injection

public interface INotificationSender
{
    void Send(string message);
}

public class EmailSender : INotificationSender
{
    public void Send(string message)
    {
        // логіка відправки email
    }
}

public class NotificationService
{
    private readonly INotificationSender _sender;

    public NotificationService(INotificationSender sender)
    {
        _sender = sender;
    }

    public void Notify(string message)
    {
        _sender.Send(message);
    }
}

3. Переваги застосування DIP

1. легко додати SmsSender або PushSender

2. бізнес-логіка не знає деталей реалізації

3. простіша підтримка коду

4. можливість підміни залежностей у тестах

Як ISP допомагає DI та тестуванню

1. не потрібно реалізовувати “зайві” методи
2. тести стають простими та зрозумілими
3. DI працює з мінімальними контрактами

Поєднання ISP та DIP робить систему гнучкою, тестованою та стійкою до змін.

Висновок
ISP дозволяє створювати вузькі, цілеспрямовані інтерфейси, а DIP — будувати систему навколо абстракцій, а не конкретних класів.
