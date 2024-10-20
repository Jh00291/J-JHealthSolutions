using J_JHealthSolutions.DAL;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;

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

        private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable the Edit button only if a row is selected
            EditButton.IsEnabled = PatientsDataGrid.SelectedItem != null;
            DeleteButton.IsEnabled = PatientsDataGrid.SelectedItem != null;
        }

        private void LoadPatients()
        {
                var patients = _patientDal.GetPatients();
                PatientsDataGrid.ItemsSource = patients;
        }

        // Add Patient
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            var addPatientWindow = new AddEditPatientWindow();  // Instantiate AddEditPatientWindow
            bool? dialogResult = addPatientWindow.ShowDialog();  // Show the window as a modal dialog

            if (dialogResult == true)  // Check if DialogResult is true
            {
                // Refresh the data grid after adding a patient
                LoadPatients();
            }
        }

        // Edit Patient
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var editPatientWindow = new AddEditPatientWindow(SelectedPatient);  // Pass the selected patient to the window
                bool? dialogResult = editPatientWindow.ShowDialog();  // Show the window as a modal dialog

                if (dialogResult == true)  // Check if DialogResult is true
                {
                    // Refresh the data grid after editing a patient
                    LoadPatients();
                }
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
                    LoadPatients();  // Refresh the DataGrid after deletion
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
                return (Patient)PatientsDataGrid.SelectedItem;  // Return the currently selected patient
            }
        }
    }
}
