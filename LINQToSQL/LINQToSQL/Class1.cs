using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQToSQL
{
    public class Class1
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public void add()
        {
            db.CustomerInfo.DeleteOnSubmit(new CustomerInfo());
            db.SubmitChanges();
        }
    }
}
