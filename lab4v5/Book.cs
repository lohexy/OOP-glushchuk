using System;

namespace lab4v5
{
    public class Book : Publication
    {
        public string Author { get; private set; }

        public Book(string title, string author, int pages)
            : base(title, pages)
        {
            Author = author;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Книга: {Title} ({Author}), {Pages} сторінок");
        }
    }
}
