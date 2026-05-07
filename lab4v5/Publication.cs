using System;

namespace lab4v5
{
    public abstract class Publication : IStorable
    {
        public string Title { get; protected set; }
        public int Pages { get; protected set; }

        public Publication(string title, int pages)
        {
            Title = title;
            Pages = pages;
        }

        public abstract void ShowInfo();
    }
}

