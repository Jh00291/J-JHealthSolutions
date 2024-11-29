using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.DAL
{
    public class AdminDal
    {
        public DataTable ExecuteQuery(string query)
        {
            try
            {
                using (var connection = new MySqlConnection(Connection.ConnectionString()))
                {
                    connection.Open();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            DataTable resultTable = new DataTable();
                            adapter.Fill(resultTable);
                            return resultTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                throw new Exception($"Error executing query: {ex.Message}", ex);
            }
        }
    }
}
