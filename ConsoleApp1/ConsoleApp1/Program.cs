using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {

        public static void HandleMultipleExceptions(string a, string b)
        {
            // TODO
            int[] numbers = { 1, 2, 3 };
            int result = 0;
            try
            {
                int c = int.Parse(a);
                result = numbers[int.Parse(b)];
                Console.WriteLine(result);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid format.");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Index out of range.");
            }
            
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter an index:");
            string input = Console.ReadLine();
            HandleMultipleExceptions("4", input);
           Console.ReadKey();
        }
    }
}
