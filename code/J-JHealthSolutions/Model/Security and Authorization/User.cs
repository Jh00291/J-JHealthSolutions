using System;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents a user in the system, including their personal information and role.
    /// </summary>
    public class User
    {
        private int _userId;
        private string _username;
        private UserRole _role;
        private string _fname;
        private string _lname;

        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int UserId
        {
            get => _userId;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("UserId cannot be less than 0");
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
        /// First name of the user, cannot be null or empty.
        /// </summary>
        public string Fname
        {
            get => _fname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("First name cannot be empty or null.");
                }
                _fname = value;
            }
        }

        /// <summary>
        /// Last name of the user, cannot be null or empty.
        /// </summary>
        public string Lname
        {
            get => _lname;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Last name cannot be empty or null.");
                }
                _lname = value;
            }
        }

        /// <summary>
        /// Constructor to create a User object with required fields.
        /// </summary>
        /// <param name="userId">Unique user identifier</param>
        /// <param name="username">Unique username</param>
        /// <param name="password">Password of the user</param>
        /// <param name="role">Role of the user</param>
        /// <param name ="fname">First name of the user</param>
        /// <param name ="lname">Last name of the user</param>
        public User(int userId, string username, UserRole role, string fname, string lname)
        {
            UserId = userId;
            Username = username;
            Role = role;
            Fname = fname;
            Lname = lname;
        }

        public User()
        {
        }
    }
}
