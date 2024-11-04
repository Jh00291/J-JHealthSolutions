using System;
using System.Data;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class UserDal
    {
        /// <summary>
        /// Attempt to log in a user by verifying their username and password.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password (should be hashed in a real scenario)</param>
        /// <returns>The User object if credentials are correct, otherwise null</returns>
        public User Login(string username, string password)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    u.user_id AS UserId, 
                    u.username AS Username, 
                    u.role AS Role,
                    e.f_name AS Fname, 
                    e.l_name AS Lname
                FROM User u
                INNER JOIN Employee e ON u.user_id = e.user_id
                WHERE u.username = @username AND u.password = @password;
            ";

            var parameters = new { username, password };

            var user = connection.QuerySingleOrDefault<User>(query, parameters);

            return user;
        }
    }
}