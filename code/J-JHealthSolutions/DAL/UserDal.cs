using System;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class UserDal
    {
        // Hashes a password using SHA256
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Login method: Verifies username and hashed password
        public User Login(string username, string plainTextPassword)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            // Hash the input password
            string hashedPassword = HashPassword(plainTextPassword);

            var query = @"
                SELECT 
                    u.user_id AS UserId, 
                    u.username AS Username, 
                    u.role AS Role,
                    e.f_name AS Fname, 
                    e.l_name AS Lname
                FROM User u
                INNER JOIN Employee e ON u.user_id = e.user_id
                WHERE u.username = @username AND u.password = @hashedPassword;
            ";

            var parameters = new { username, hashedPassword };

            return connection.QuerySingleOrDefault<User>(query, parameters);
        }

        // Adds a new user with hashed password
        public void AddUser(User user, string plainTextPassword)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            string hashedPassword = HashPassword(plainTextPassword);

            var query = @"
                INSERT INTO User (username, password, role)
                VALUES (@username, @hashedPassword, @role);
            ";

            var parameters = new
            {
                username = user.Username,
                hashedPassword,
                role = user.Role
            };

            connection.Execute(query, parameters);
        }

        // Migrates existing plaintext passwords to hashed passwords
        public void MigratePasswords()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var users = connection.Query<dynamic>("SELECT user_id, password FROM User;");

            foreach (var user in users)
            {
                string hashedPassword = HashPassword(user.password);
                connection.Execute(
                    "UPDATE User SET password = @hashedPassword WHERE user_id = @userId;",
                    new { hashedPassword, userId = user.user_id }
                );
            }
        }
    }
}
