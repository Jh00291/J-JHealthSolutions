namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents a singleton session manager that handles user login and logout.
    /// The <see cref="UserSession"/> class ensures that only one instance of the session exists
    /// during the application runtime.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// The singleton instance of the <see cref="UserSession"/> class.
        /// </summary>
        private static UserSession _instance;

        /// <summary>
        /// Gets the singleton instance of the <see cref="UserSession"/>.
        /// If the instance does not exist, it will be created.
        /// </summary>
        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserSession();
                return _instance;
            }
        }

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private UserSession() { }

        /// <summary>
        /// Gets the currently logged-in <see cref="User"/> for this session.
        /// If no user is logged in, this will return <c>null</c>.
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Logs in a specified <see cref="User"/> to the current session.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to be logged in.</param>
        public void Login(User user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// Logs out the current user, effectively ending the session.
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
        }

        /// <summary>
        /// Indicates whether a user is currently logged into the session.
        /// Returns <c>true</c> if a user is logged in; otherwise, <c>false</c>.
        /// </summary>
        public bool IsLoggedIn => CurrentUser != null;
    }
}
