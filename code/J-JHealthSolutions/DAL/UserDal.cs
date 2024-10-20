using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;

namespace J_JHealthSolutions.DAL;

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
        User user = null;
        using var connection = new MySqlConnection(Connection.ConnectionString());

        connection.Open();
        var query = "SELECT user_id, username, password, role FROM User WHERE username = @username AND password = @password;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var userIdOrdinal = reader.GetOrdinal("user_id");
            var usernameOrdinal = reader.GetOrdinal("username");
            var roleOrdinal = reader.GetOrdinal("role");

            user = CreateUser(reader, userIdOrdinal, usernameOrdinal, roleOrdinal);
        }

        return user;
    }

    private static User CreateUser(MySqlDataReader reader, int userIdOrdinal, int usernameOrdinal, int roleOrdinal)
    {
        return new User
        {
            UserId = reader.GetInt32(userIdOrdinal),
            Username = reader.GetString(usernameOrdinal),
            Role = Enum.TryParse<UserRole>(reader.GetString(roleOrdinal), true, out var userRole)
                ? userRole
                : UserRole.Other
        };
    }

}