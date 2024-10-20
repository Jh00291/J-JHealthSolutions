using System;

namespace J_JHealthSolutions.Model
{
    public class User
    {
        private int _userId;
        private string _username;
        private UserRole _role;

        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int UserId
        {
            get => _userId;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("UserId must be greater than zero.");
                }
                _userId = value;
            }
        }

        /// <summary>
        /// Username of the user (must be unique)
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be empty or null.");
                }

                if (value.Length < 3)
                {
                    throw new ArgumentException("Username must be at least 3 characters long.");
                }

                _username = value;
            }
        }

        /// <summary>
        /// Role of the user (e.g., Doctor, Nurse, Administrator, etc.)
        /// </summary>
        public UserRole Role
        {
            get => _role;
            set
            {
                if (!Enum.IsDefined(typeof(UserRole), value))
                {
                    throw new ArgumentException("Invalid role specified.");
                }
                _role = value;
            }
        }

        /// <summary>
        /// Constructor to create a User object with required fields.
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="username">Unique username</param>
        /// <param name="password">Password of the user</param>
        /// <param name="role">Role of the user</param>
        public User(int userId, string username, UserRole role)
        {
            UserId = userId;
            Username = username;
            Role = role;
        }

        public User()
        {
        }
    }
}
