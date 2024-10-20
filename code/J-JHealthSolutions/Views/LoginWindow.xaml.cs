﻿using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            var userDal = new UserDal();
            User loggedInUser = userDal.Login(username, password);

            if (loggedInUser != null)
            {
                // Login successful
                MainWindow mainWindow = new MainWindow(loggedInUser);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // Login failed
                MessageBox.Show("Invalid username or password");
            }
        }
    }

}