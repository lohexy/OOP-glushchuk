using System;
using System.Collections.Generic;

namespace lab2v5
{
    public class bankaccount
    {
        private decimal balance;
        private List<string> transactions = new List<string>();

        public decimal Balance
        {
            get => balance;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Баланс не може бути від’ємним!");
                balance = value;
            }
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= transactions.Count)
                    return "Немає такої транзакції.";
                return transactions[index];
            }
        }

        public bankaccount(decimal initialBalance = 0)
        {
            Balance = initialBalance;
            transactions.Add($"Відкрито рахунок з балансом: {Balance:C}");
        }

        public static bankaccount operator +(bankaccount account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Сума поповнення не може бути від’ємною!");
            account.Balance += amount;
            account.transactions.Add($"Поповнення: +{amount:C}");
            return account;
        }

        public static bankaccount operator -(bankaccount account, decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Сума зняття не може бути від’ємною!");
            if (account.Balance - amount < 0)
                throw new InvalidOperationException("Недостатньо коштів!");
            account.Balance -= amount;
            account.transactions.Add($"Зняття: -{amount:C}");
            return account;
        }

        public static bool operator >(bankaccount a, bankaccount b) => a.Balance > b.Balance;
        public static bool operator <(bankaccount a, bankaccount b) => a.Balance < b.Balance;

        public void PrintInfo()
        {
            Console.WriteLine($"Баланс: {Balance:C}");
        }

        public void ShowTransactions()
        {
            Console.WriteLine("Історія транзакцій:");
            foreach (var t in transactions)
                Console.WriteLine($" - {t}");
        }
    }
}
