using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for MainMenuControl.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl
    {
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
            DependencyProperty.Register("UserRole", typeof(string), typeof(MainMenuControl), new PropertyMetadata(string.Empty, OnUserRoleChanged));

        /// <summary>
        /// The role of the current user, which determines what menu options are available.
        /// </summary>
        public string UserRole
        {
            get { return (string)GetValue(UserRoleProperty); }
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
            control.UpdateManageEmployeesVisibility();
        }

        /// <summary>
        /// Updates the visibility of the Manage Employees button based on the user's role.
        /// </summary>
        private void UpdateManageEmployeesVisibility()
        {
            //Todo add the admin role
            ManageEmployeesButton.Visibility = UserRole == "ToDo" ? Visibility.Visible : Visibility.Collapsed;
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

    }
}
