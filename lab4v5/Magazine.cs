using System;

namespace lab4v5
{
    public class Magazine : Publication
    {
        public int IssueNumber { get; private set; }

        public Magazine(string title, int issueNumber, int pages)
            : base(title, pages)
        {
            IssueNumber = issueNumber;
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Журнал: {Title} (№{IssueNumber}), {Pages} сторінок");
        }
    }
}
