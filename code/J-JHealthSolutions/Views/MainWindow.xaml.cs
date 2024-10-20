using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User CurrentUser { get; set; }

        public MainWindow()
        {

            InitializeComponent();

            CurrentUser = new User("123", "hello", UserRole.Administrator);

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId;
            userInfoControl.FullName = $"{CurrentUser.Username}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();
        }

        public MainWindow(User currentUser)
        {
            InitializeComponent();

            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            CurrentUser = new User("123", "hello", UserRole.Nurse);

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId;
            userInfoControl.FullName = $"{CurrentUser.Username}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();

        }

    }
}
