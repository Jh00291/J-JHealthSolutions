using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public ICommand LogOutCommand { get; private set; }

        public MainWindow()
        {

            InitializeComponent();

            CurrentUser = new User("123", "hello", UserRole.Administrator);

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId;
            userInfoControl.FullName = $"{CurrentUser.Username}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();

            InitializeCommands();

            mainMenuControl.LogOutCommand = LogOutCommand;
        }

        private void InitializeCommands()
        {
            LogOutCommand = new RelayCommand(ExecuteLogOut);
        }

        public MainWindow(User currentUser)
        {
            InitializeComponent();

            //Todo verify that the user is not null and the proper user is passed
            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId;
            userInfoControl.FullName = $"{CurrentUser.Username}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();

            InitializeCommands();

            mainMenuControl.LogOutCommand = LogOutCommand;

        }

        private void ExecuteLogOut(object parameter)
        {

            // Show the LoginWindow
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }

    }
}
