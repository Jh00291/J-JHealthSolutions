using MySql.Data.MySqlClient;
using System.Data;

namespace J_JHealthSolutions.DAL.Domain
{
    public class AdminDal
    {
        public DataTable ExecuteQuery(string query)
        {
            try
            {
                using var connection = new MySqlConnection(Connection.ConnectionString());
                connection.Open();

                using var command = new MySqlCommand(query, connection);
                using var adapter = new MySqlDataAdapter(command);
                DataTable resultTable = new DataTable();
                adapter.Fill(resultTable);
                return resultTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}", ex);
            }
        }
    }
}
