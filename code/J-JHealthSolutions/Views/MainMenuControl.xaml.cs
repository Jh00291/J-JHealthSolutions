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
        public MainMenuControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public static readonly DependencyProperty UserRoleProperty =
            DependencyProperty.Register("UserRole", typeof(string), typeof(MainMenuControl), new PropertyMetadata(string.Empty, OnUserRoleChanged));

        public string UserRole
        {
            get { return (string)GetValue(UserRoleProperty); }
            set { SetValue(UserRoleProperty, value); }
        }



        private static void OnUserRoleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MainMenuControl;
            control.UpdateManageEmployeesVisibility();
        }

        private void UpdateManageEmployeesVisibility()
        {
            ManageEmployeesButton.Visibility = UserRole == Model.UserRole.Administrator.ToString() ? Visibility.Visible : Visibility.Collapsed;
        }

        public static readonly DependencyProperty LogOutCommandProperty =
            DependencyProperty.Register("LogOutCommand", typeof(ICommand), typeof(MainMenuControl));

        public ICommand LogOutCommand
        {
            get { return (ICommand)GetValue(LogOutCommandProperty); }
            set { SetValue(LogOutCommandProperty, value); }
        }

    }
}
