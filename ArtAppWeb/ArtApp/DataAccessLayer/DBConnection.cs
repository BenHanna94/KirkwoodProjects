using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataAccessLayer
{
   internal static class DBConnection
    {
        private static string connectionString = @"Data Source=ZEPHYR-PC;Initial Catalog=ArtAppDB;Integrated Security=True";

        // This is the only method, and the only class, where the connection string is allowed to appear  
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
