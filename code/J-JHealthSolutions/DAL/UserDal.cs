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
        var query = @"
            SELECT u.user_id, u.username, u.password, u.role,
                   e.f_name, e.l_name
            FROM User u
            INNER JOIN Employee e ON u.user_id = e.user_id
            WHERE u.username = @username AND u.password = @password;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var userIdOrdinal = reader.GetOrdinal("user_id");
            var usernameOrdinal = reader.GetOrdinal("username");
            var roleOrdinal = reader.GetOrdinal("role");
            var firstNameOrdinal = reader.GetOrdinal("f_name");
            var lastNameOrdinal = reader.GetOrdinal("l_name");

            user = CreateUser(reader, userIdOrdinal, usernameOrdinal, roleOrdinal, firstNameOrdinal, lastNameOrdinal);
        }


        return user;
    }

    /// <summary>
    /// Create a User object from a data reader
    /// </summary>
    /// <param name="reader">The MySqlDataReader containing user data</param>
    /// <param name="userIdOrdinal">Ordinal position of user ID</param>
    /// <param name="usernameOrdinal">Ordinal position of username</param>
    /// <param name="roleOrdinal">Ordinal position of role</param>
    /// <param name="firstNameOrdinal">Ordinal position of first name</param>
    /// <param name="lastNameOrdinal">Ordinal position of last name</param>
    /// <returns>A new User object populated with the data from the reader</returns>
    private User CreateUser(MySqlDataReader reader, int userIdOrdinal, int usernameOrdinal, int roleOrdinal, int firstNameOrdinal, int lastNameOrdinal)
    {
        return new User
        {
            UserId = reader.GetInt32(userIdOrdinal),
            Username = reader.GetString(usernameOrdinal),
            Role = Enum.TryParse<UserRole>(reader.GetString(roleOrdinal), true, out var userRole)
                ? userRole
                : UserRole.Other,
            Fname = reader.GetString(firstNameOrdinal),
            Lname = reader.GetString(lastNameOrdinal)
        };
    }

}