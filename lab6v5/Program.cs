using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab6v5 
{
    // Делегат для арифметичних операцій
    public delegate double MathOperation(double a, double b);

    // Клас Employee 
    public class Employee
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        public Employee(string name, string position, decimal salary)
        {
            Name = name;
            Position = position;
            Salary = salary;
        }

        public override string ToString()
        {
            return $"{Name} | {Position} | ${Salary}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("1.Базові делегати та анонімні методи");

            //Анонімний метод, використання delegate
            MathOperation averageOp = delegate (double x, double y)
            {
                Console.WriteLine("Обчислення середнього арифметичного (анонімний метод)");
                return (x + y) / 2;
            };

            double resAvg = averageOp(10, 20);
            Console.WriteLine($"Результат: {resAvg}\n");

            //Лямбда-вираз
            MathOperation multiplyOp = (x, y) => x * y;
            Console.WriteLine($"Множення (лямбда): 5 * 4 = {multiplyOp(5, 4)}\n");

            Console.WriteLine("2.Вбудовані делегати (Func, Action, Predicate)");

            List<Employee> employees = new List<Employee>
            {
                new Employee("Олександр", "Junior Dev", 8000),
                new Employee("Марія", "Senior Dev", 15000),
                new Employee("Іван", "Manager", 12000),
                new Employee("Олена", "QA Engineer", 9500),
                new Employee("Петро", "Team Lead", 20000)
            };

            // Func<T, TResult>: вибрати працівників із зарплатою > 10 000
            Func<Employee, bool> isHighSalary = e => e.Salary > 10000;

            //Predicate<T>: Перевірити, чи є працівник менеджером
            Predicate<Employee> isManager = e => e.Position == "Manager";

            //Action<T>: Вивід у консоль
            Action<Employee> printEmployee = e => Console.WriteLine($"[Action Output]: {e}");

            Console.WriteLine("Співробітники із зарплатою > 10 000 (використання Func + Action):");
            
            //Фільтрація для роботи делегатів
            foreach (var emp in employees)
            {
                if (isHighSalary(emp)) // Виклик Func
                {
                    printEmployee(emp); // Виклик Action
                }
            }
            Console.WriteLine();

            Console.WriteLine("\nПошук менеджерів (використання Predicate):");
            
            //Виклик Predicate<T>
            List<Employee> managers = employees.FindAll(isManager);

            foreach (var m in managers)
            {
                printEmployee(m);
            }
            Console.WriteLine();

            Console.WriteLine("3. LINQ операції");

            //Where + OrderBy 
            //Фільтрація та сортування
            var sortedJuniors = employees
                .Where(e => e.Salary < 10000) 
                .OrderBy(e => e.Name);

            Console.WriteLine("Співробітники з зарплатою < 10 000 (відсортовані за ім'ям):");
            foreach (var item in sortedJuniors)
            {
                Console.WriteLine($" - {item}");
            }

            //Select
            //Список посад
            var positions = employees.Select(e => e.Position);
            Console.WriteLine($"\nСписок всіх посад: {string.Join(", ", positions)}");

            //Aggregate 
            //Обчислення загального фонду заробітної плати
            decimal totalPayroll = employees.Aggregate(0m, (acc, emp) => acc + emp.Salary);
            
            Console.WriteLine($"\nЗагальний зарплатний фонд (Aggregate): {totalPayroll}$");

            Console.ReadKey();
        }
    }
}