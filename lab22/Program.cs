using System;

namespace lab22
{
    //ПОРУШЕННЯ LSP
    public class Printer
    {
        public virtual void PrintColorImage(string content)
        {
            Console.WriteLine($"Printing color image: {content}");
        }
    }

    public class BlackAndWhitePrinter : Printer
    {
        public override void PrintColorImage(string content)
        {
            // ПОРУШЕННЯ:Не можна виконати обіцянку базового класу
            throw new NotSupportedException("Error: BlackAndWhitePrinter cannot print in color!");
        }
    }

    // ЧАСТИНА 2: РЕФАКТОРИНГ
    public interface IPrinter
    {
        void PrintGreyScale(string content);
    }

    public interface IColorPrinter : IPrinter
    {
        void PrintColor(string content);
    }

    public class BasicPrinter : IPrinter
    {
        public void PrintGreyScale(string content) => 
            Console.WriteLine($"[B&W] Printing: {content}");
    }

    public class HighEndPrinter : IColorPrinter
    {
        public void PrintGreyScale(string content) => 
            Console.WriteLine($"[B&W mode] Printing: {content}");

        public void PrintColor(string content) => 
            Console.WriteLine($"[Color mode] Printing: {content}");
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LSP Violation Demo");
            DemoLSPViolation();

            Console.WriteLine("\nLSP Compliant Solution");
            DemoLSPCorrect();
            
            Console.ReadLine();
        }

        static void ClientPrintService(Printer printer)
        {
            try
            {
                printer.PrintColorImage("Family_Photo.jpg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client encountered a problem: {ex.Message}");
            }
        }

        static void DemoLSPViolation()
        {
            Printer genericPrinter = new Printer();
            Printer bwPrinter = new BlackAndWhitePrinter();

            Console.WriteLine("Working with generic printer:");
            ClientPrintService(genericPrinter);

            Console.WriteLine("Working with B&W printer (substituted):");
            ClientPrintService(bwPrinter); 
        }

        static void SafeColorPrintService(IColorPrinter printer)
        {
            printer.PrintColor("Vacation_Photo.png");
        }

        static void DemoLSPCorrect()
        {
            IPrinter basic = new BasicPrinter();
            IColorPrinter fancy = new HighEndPrinter();

            basic.PrintGreyScale("Document.pdf");
            
            SafeColorPrintService(fancy);
            
            Console.WriteLine("Refactored system works correctly and safely.");
        }
    }
}