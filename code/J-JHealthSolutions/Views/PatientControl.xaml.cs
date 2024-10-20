using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace J_JHealthSolutions.Views
{
    public partial class PatientControl : UserControl
    {
        private PatientDal _patientDal = new PatientDal();

        public PatientControl()
        {
            InitializeComponent();
            LoadPatients();
        }

        private void LoadPatients()
        {
                var patients = _patientDal.GetPatients();
                PatientsDataGrid.ItemsSource = patients;
        }

        // Add Patient
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            var addPatientWindow = new AddPatientWindow();  // Instantiate AddPatientWindow
            addPatientWindow.ShowDialog();  // Show the window as a modal dialog
        }

        // Edit Patient
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var editPatientWindow = new EditPatientWindow(SelectedPatient);  // Pass the selected patient to the window
                editPatientWindow.ShowDialog();  // Show the window as a modal dialog
            }
            else
            {
                MessageBox.Show("Please select a patient to edit.");
            }
        }

        // Delete Patient
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {SelectedPatient.FName} {SelectedPatient.LName}?",
                                              "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _patientDal.DeletePatient(SelectedPatient.PatientId.Value);  // Use the DAL to delete the patient
                    MessageBox.Show("Patient deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a patient to delete.");
            }
        }

        private Patient SelectedPatient  // Add a property to track the selected patient from the UI (e.g., a ListBox or DataGrid)
        {
            get
            {
                // Return the currently selected patient from your UI component (ListBox, DataGrid, etc.)
                // For example: return (Patient)patientListBox.SelectedItem;
                return null;  // Placeholder, replace with your actual selection logic
            }
        }
    }
}
