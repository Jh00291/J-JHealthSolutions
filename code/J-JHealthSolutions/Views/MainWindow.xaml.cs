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
        /// <summary>
        /// The currently logged-in user.
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// The command that handles the logout process.
        /// </summary>
        public ICommand LogOutCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainWindow for testing or default purposes, setting up a default user.
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            CurrentUser = new User(123, "hello", UserRole.Administrator, "Jason", "Nunez");

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId.ToString();
            userInfoControl.FullName = $"{CurrentUser.Fname} {CurrentUser.Lname}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();

            InitializeCommands();

            SubscribeToEvents();

            mainMenuControl.LogOutCommand = LogOutCommand;
        }

        private void SubscribeToEvents()
        {
            mainMenuControl.ManageVisitSelected += MainMenuControl_ManageVisitSelected;
            mainMenuControl.ManagePatientsSelected += MainMenuControl_ManagePatientsSelected;
            mainMenuControl.ManageAppointmentsSelected += MainContentControl_ManageAppointmentSelected;
        }

        private void MainContentControl_ManageAppointmentSelected(object? sender, EventArgs e)
        {
            MainContentControl.Content = new AppointmentControl();
        }

        private void MainMenuControl_ManagePatientsSelected(object? sender, EventArgs e)
        {
            MainContentControl.Content = new PatientControl();
        }

        private void MainMenuControl_ManageVisitSelected(object? sender, EventArgs e)
        {
            MainContentControl.Content = new VisitControl();
        }

        /// <summary>
        /// Initializes the commands used in the MainWindow.
        /// </summary>
        private void InitializeCommands()
        {
            LogOutCommand = new RelayCommand(ExecuteLogOut);
        }

        /// <summary>
        /// Initializes a new instance of the MainWindow, passing in the current user after a successful login.
        /// </summary>
        /// <param name="currentUser">The user who has logged in</param>
        public MainWindow(User currentUser)
        {
            InitializeComponent();

            //Todo verify that the user is not null and the proper user is passed
            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            // Set user information in UserInfoControl
            userInfoControl.UserId = CurrentUser.UserId.ToString();
            userInfoControl.FullName = $"{CurrentUser.Fname} {CurrentUser.Lname}";

            // Pass the UserRole to MainMenuControl
            mainMenuControl.UserRole = CurrentUser.Role.ToString();

            InitializeCommands();

            mainMenuControl.LogOutCommand = LogOutCommand;

        }

        /// <summary>
        /// Executes the logout process, closing the current window and showing the LoginWindow.
        /// </summary>
        /// <param name="parameter">The parameter passed to the command (can be null)</param>
        private void ExecuteLogOut(object parameter)
        {

            var loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }

    }
}
