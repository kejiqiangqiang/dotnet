using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Class1
    {
        LINQtoSQLDataContext db = new LINQtoSQLDataContext();
        Entities ef = new Entities();

        public void adonet_get()
        {
            string sqlconnectstring = "Data Source=192.168.3.179;Initial Catalog=EFOS.Master;Persist Security Info=true;User ID=sa;password=111111;TimeOut=210";
            string querystring = "select * from CustomerInfo where CustomerID>@CustomerID";
            int paramValue = 1;
            using (SqlConnection connection = new SqlConnection(sqlconnectstring))
            {
                SqlCommand command = new SqlCommand(querystring, connection);
                command.Parameters.AddWithValue("@CustomerID", paramValue);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = reader[0];
                        var j = reader[1];
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

                var CommandText=command.CommandText;

            }
        }
        public void linqtosql_get()
        {
            //var models0 = db.Acc_Role.ToList();
            var models = db.Acc_Role;

            var query = from model in models
                        where model.FRoleID > 0
                        select model;
            //db.Acc_Role.DeleteOnSubmit(new Acc_Role() { FRoleID = 1 });
            //db.SubmitChanges();
            var CommandText = db.GetCommand(query).CommandText;

            db.Log = Console.Out;

            //var models0 = db.Acc_Role.ToList();立即加载执行后，foreach中直接跳过了？
            foreach (Acc_Role role in query)
            {
                Console.WriteLine("Name: {0}", role.RoleName);
                Console.WriteLine("");
            }

        }

        public void ef_get()
        {
            //var users0 = ef.Acc_UserInfo.ToList();
            var users =  ef.Acc_UserInfo;
            var query = from user in users
                        where user.UserID > 0
                        select user;


            string CommandText;
            

            CommandText = query.ToString();

            ef.Database.Log = (sql) => { CommandText = sql; };

            //var users0 = ef.Acc_UserInfo.ToList();立即加载执行后，foreach中直接跳过了？
            foreach (Acc_UserInfo user in query)
            {
                Console.WriteLine("Name: {0}", user.UserName);
                Console.WriteLine("Account: {0}", user.Account);
                Console.WriteLine("");
            }
        }

    }
}
