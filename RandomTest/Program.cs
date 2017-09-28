using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(random.NextDouble());
            }

            Console.ReadKey();
        }
    }
}
