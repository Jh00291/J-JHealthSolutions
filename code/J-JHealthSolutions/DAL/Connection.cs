using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public static class Connection
    {
        public static string ConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                //Todo verify connection string
                Server = "cs-dblab01.uwg.westga.edu",
                Database = "cs3230f24a",
                UserID = "cs3230f24a",
                Password = "tex.U47VGb>*eq.VQ)K{",
                Port = 3306
            };

            return builder.ToString();

        }
    }
}
