using System;

namespace lab2v5
{
    class program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            BankAccount acc1 = new BankAccount(1000);
            BankAccount acc2 = new BankAccount(500);

            acc1 += 200;   
            acc1 -= 150;   
            acc2 += 50;

            acc1.PrintInfo();
            acc2.PrintInfo();

            Console.WriteLine(acc1 > acc2 ? "acc1 має більший баланс" : "acc2 має більший баланс");

            Console.WriteLine("\nІндексатор:");
            Console.WriteLine(acc1[0]);
            Console.WriteLine(acc1[2]);

            Console.WriteLine();
            acc1.ShowTransactions();
        }
    }
}
