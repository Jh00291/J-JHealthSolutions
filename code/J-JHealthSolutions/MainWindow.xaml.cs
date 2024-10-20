using System;
using System.Threading.Tasks;
using System.Windows;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UserDal _userDal;

        public MainWindow()
        {
            InitializeComponent();
            _userDal = new UserDal();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text.Trim();
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                User user = _userDal.Login(username, password);

                if (user != null)
                {
                    MessageBox.Show($"Welcome, {user.Username}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
