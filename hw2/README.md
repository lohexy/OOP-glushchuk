Порушення принципу підстановки Лісков (LSP)
Що таке LSP

Принцип підстановки Лісков (Liskov Substitution Principle) сформульований Barbara Liskov.

Суть принципу:
Об’єкти підкласу повинні замінювати об’єкти базового класу без порушення коректності роботи програми.

Якщо клас B наслідується від A, то B не повинен змінювати очікувану поведінку A.        

1. Приклад: Доступ до файлів (ReadOnly vs Writable)

Це класичне порушення контракту базового класу.
Порушення 

Клас `ReadOnlyFile` наслідує `File`, але кидає виключення в методі запису. Клієнтський код, який очікує, що будь-який файл можна змінити, зламається.

public class File {
    public virtual void Write(string content) => Console.WriteLine("Writing to file...");
}

public class ReadOnlyFile : File {
    public override void Write(string content) => 
        throw new UnauthorizedAccessException("Cannot write to a read-only file!");
}

Рефакторинг

Ми розділяємо обов'язки за допомогою інтерфейсів. Тепер ми не обіцяємо запис там, де його не може бути.

public interface IReadable {
    void Read();
}

public interface IWritable {
    void Write(string content);
}

public class ReadOnlyFile : IReadable {
    public void Read() => Console.WriteLine("Reading content...");
}

public class WritableFile : IReadable, IWritable {
    public void Read() => Console.WriteLine("Reading content...");
    public void Write(string content) => Console.WriteLine("Writing content...");
}

Тепер методи, які потребують запису, прийматимуть `IWritable`. Випадково передати туди `ReadOnlyFile` неможливо на етапі компіляції.

2. Приклад: Обробка платежів (Посилення передумов)

Порушення виникає, коли підклас вимагає більше даних для роботи, ніж базовий клас.

Порушення

`PayPalProcessor` вимагає встановленого `Email`, інакше він не може виконати базовий метод `Process()`.

public class PaymentProcessor {
    public virtual void Process(decimal amount) => Console.WriteLine($"Paid {amount}");
}

public class PayPalProcessor : PaymentProcessor {
    public string Email { get; set; }
    public override void Process(decimal amount) {
        if (string.IsNullOrEmpty(Email)) throw new InvalidOperationException("Email is required!");
        Console.WriteLine($"Paid {amount} via PayPal to {Email}");
    }
}

Рефакторинг

Використовуємо об'єкт контексту або змінюємо ієрархію так, щоб залежності передавалися через конструктор. Це гарантує, що об'єкт завжди готовий до роботи (обіцянки базового класу виконуються).

public abstract class PaymentProcessor {
    public abstract void Process(decimal amount);
}

public class CashProcessor : PaymentProcessor {
    public override void Process(decimal amount) => Console.WriteLine($"Paid {amount} in cash.");
}

public class PayPalProcessor : PaymentProcessor {
    private readonly string _email;
    // Залежність передається при створенні, контракт методу Process залишається чистим
    public PayPalProcessor(string email) => _email = email;

    public override void Process(decimal amount) => 
        Console.WriteLine($"Paid {amount} via PayPal to {_email}");
}

Тепер будь-який об'єкт, що наслідує `PaymentProcessor`, гарантовано виконає `Process()` без додаткових перевірок «а чи не забули ми щось налаштувати».
