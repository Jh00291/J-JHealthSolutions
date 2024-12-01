using J_JHealthSolutions.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for MainMenuControl.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl
    {

        public event EventHandler ManageVisitSelected;
        public event EventHandler ManagePatientsSelected;
        public event EventHandler ManageAppointmentsSelected;
        public event EventHandler AdminDashboardSelected;

        /// <summary>
        /// Initializes a new instance of the MainMenuControl class.
        /// </summary>
        public MainMenuControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Dependency property for the UserRole, which controls the visibility of certain menu options.
        /// </summary>
        public static readonly DependencyProperty UserRoleProperty =
            DependencyProperty.Register(
                "UserRole",
                typeof(UserRole),
                typeof(MainMenuControl),
                new PropertyMetadata(UserRole.Other, OnUserRoleChanged));

        public UserRole UserRole
        {
            get { return (UserRole)GetValue(UserRoleProperty); }
            set { SetValue(UserRoleProperty, value); }
        }


        /// <summary>
        /// Callback method that is triggered when the UserRole property changes.
        /// </summary>
        /// <param name="d">The object whose property changed</param>
        /// <param name="e">Details about the property change</param>
        private static void OnUserRoleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MainMenuControl;
            control?.UpdateAdminDashboardVisibility();
        }

        private void UpdateAdminDashboardVisibility()
        {
            if (AdminDashboardButton != null)
            {
                AdminDashboardButton.Visibility = UserRole == UserRole.Administrator
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }


        /// <summary>
        /// Dependency property for the LogOutCommand, which handles the logout action.
        /// </summary>
        public static readonly DependencyProperty LogOutCommandProperty =
            DependencyProperty.Register("LogOutCommand", typeof(ICommand), typeof(MainMenuControl));

        /// <summary>
        /// The command that is triggered when the user logs out.
        /// </summary>
        public ICommand LogOutCommand
        {
            get { return (ICommand)GetValue(LogOutCommandProperty); }
            set { SetValue(LogOutCommandProperty, value); }
        }

        private void ManagePatientsClick(object sender, RoutedEventArgs e)
        {
            ManagePatientsSelected?.Invoke(this, EventArgs.Empty);
        }

        private void ManageVisitsClick(object sender, RoutedEventArgs e)
        {
            ManageVisitSelected?.Invoke(this, EventArgs.Empty);
        }

        private void ManageAppointmentsClick(object sender, RoutedEventArgs e)
        {
            ManageAppointmentsSelected?.Invoke(this, EventArgs.Empty);
        }

        private void AdminDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            AdminDashboardSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
