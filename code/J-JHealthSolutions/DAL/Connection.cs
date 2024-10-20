using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    /// <summary>
    /// Provides functionality to build a connection string for MySQL database access.
    /// </summary>
    public static class Connection
    {
        /// <summary>
        /// Constructs and returns a MySQL connection string using pre-configured credentials.
        /// </summary>
        /// <returns>
        /// A string representing the MySQL connection string, which can be used to open a connection to the database.
        /// </returns>
        /// <remarks>
        /// The connection string includes the server address, database name, user ID, password, and port number.
        /// Ensure to verify and secure sensitive information like the password.
        /// </remarks>
        public static string ConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
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