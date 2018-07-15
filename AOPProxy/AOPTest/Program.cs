using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new UnitTest1();
            test.TestMethod1();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
