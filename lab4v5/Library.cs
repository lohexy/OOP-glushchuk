using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4v5
{
    public class Library
    {
        private List<IStorable> items = new List<IStorable>();

        public void AddItem(IStorable item) => items.Add(item);

        public void ShowAll()
        {
            Console.WriteLine("Вміст бібліотеки:");
            foreach (var i in items)
                i.ShowInfo();
        }

        public int TotalPages() => items.Sum(i => i.Pages);

        public double AveragePages() => items.Count == 0 ? 0 : items.Average(i => i.Pages);
    }
}
