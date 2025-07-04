using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pms
{
    internal class A6Y3
    {
        public SqlConnection con;
        string str = "Data Source=.\\SQLEXPRESS; Initial Catalog=ITA6; Integrated Security = true;";
        public void Connection()
        {
            SqlDependency.Stop(str);
            SqlDependency.Start(str);
            con = new SqlConnection(str);
            con.Open();
        }
    }
}
