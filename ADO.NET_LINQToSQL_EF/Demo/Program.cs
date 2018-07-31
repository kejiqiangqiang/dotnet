using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var bll = new Class1();
            bll.adonet_get();
            bll.linqtosql_get();
            bll.ef_get();

        }

    }
}
