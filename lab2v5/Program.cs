using System;

namespace lab2v5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            BankAccount acc1 = new BankAccount(1000);
            BankAccount acc2 = new BankAccount(500);

            acc1 += 200;
            acc1 -= 150;
            acc2 += 50;

            Console.WriteLine($"acc1 баланс: {acc1.Balance}");
            Console.WriteLine($"acc2 баланс: {acc2.Balance}");

            Console.WriteLine("\nПерша транзакція acc1:");
            Console.WriteLine(acc1[0]);

            Console.WriteLine("\nВсі транзакції acc1:");
            acc1.ShowTransactions();
        }
    }
}
