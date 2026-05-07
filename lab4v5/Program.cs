using System;

namespace lab4v5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var library = new Library();

            //Книги
            library.AddItem(new Book("Солодка Даруся", "Марія Матіос", 356));
            library.AddItem(new Book("Захар Беркут", "Іван Франко", 240));
            //Журнали
            library.AddItem(new Magazine("Український тиждень", 15, 90));
            library.AddItem(new Magazine("Forbes Україна", 5, 120));

            //вивід
            library.ShowAll();

            Console.WriteLine($"\nЗагальна кількість сторінок: {library.TotalPages()}");
            Console.WriteLine($"Середній обсяг видань: {library.AveragePages():F2}");
        }
    }
}
