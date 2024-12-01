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
        /// <summary>
        /// Hashes a plain-text password using SHA256.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>The hashed password as a Base64-encoded string.</returns>
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Logs in a user by verifying the username and hashed password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="plainTextPassword">The plain-text password to verify.</param>
        /// <returns>The user object if login is successful; otherwise, null.</returns>
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

        /// <summary>
        /// Adds a new user to the database with a hashed password.
        /// </summary>
        /// <param name="user">The user object containing username and role.</param>
        /// <param name="plainTextPassword">The plain-text password to hash and store.</param>
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

        /// <summary>
        /// Migrates existing plaintext passwords in the database to hashed passwords.
        /// </summary>
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
