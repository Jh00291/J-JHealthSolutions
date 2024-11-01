using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    public partial class AppointmentControl : UserControl
    {
        private readonly AppointmentDal _appointmentDal;
        private List<Appointment> _appointments;

        public AppointmentControl()
        {
            InitializeComponent();
            _appointmentDal = new AppointmentDal();
            LoadAppointments();
        }

        /// <summary>
        /// Loads appointments from the database and binds them to the DataGrid.
        /// </summary>
        private void LoadAppointments()
        {
            try
            {
                _appointments = _appointmentDal.GetAppointments();
                AppointmentsDataGrid.ItemsSource = _appointments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the selection change in the DataGrid to enable/disable Edit and Delete buttons.
        /// </summary>
        private void AppointmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isItemSelected = AppointmentsDataGrid.SelectedItem != null;
            EditButton.IsEnabled = isItemSelected;
        }

        /// <summary>
        /// Handles the Add button click event to add a new appointment.
        /// </summary>
        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var addAppointmentWindow = new AddEditAppointmentWindow();
            bool? dialogResult = addAppointmentWindow.ShowDialog();
            if (dialogResult == true) {
                LoadAppointments();
            }

        }

        /// <summary>
        /// Handles the Edit button click event to edit the selected appointment.
        /// </summary>
        private void EditAppointment_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
