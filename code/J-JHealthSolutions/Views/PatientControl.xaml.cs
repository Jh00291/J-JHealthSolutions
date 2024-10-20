using J_JHealthSolutions.DAL;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for managing patients, including adding, editing, and deleting patients.
    /// </summary>
    public partial class PatientControl : UserControl
    {
        private PatientDal _patientDal = new PatientDal();

        /// <summary>
        /// Initializes a new instance of the PatientControl and loads the list of patients.
        /// </summary>
        public PatientControl()
        {
            InitializeComponent();
            LoadPatients();
        }

        /// <summary>
        /// Handles the selection change event for the DataGrid, enabling or disabling the Edit and Delete buttons based on the selection.
        /// </summary>
        /// <param name="sender">The DataGrid where the selection occurred</param>
        /// <param name="e">Event arguments for the selection change</param>
        private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = PatientsDataGrid.SelectedItem != null;
            DeleteButton.IsEnabled = PatientsDataGrid.SelectedItem != null;
        }

        /// <summary>
        /// Loads the list of patients from the database and binds them to the DataGrid.
        /// </summary>
        private void LoadPatients()
        {
                var patients = _patientDal.GetPatients();
                PatientsDataGrid.ItemsSource = patients;
        }

        /// <summary>
        // /// Handles the click event to add a new patient by opening the AddEditPatientWindow in "Add" mode.
        // /// </summary>
        // /// <param name="sender">The button that was clicked</param>
        // /// <param name="e">Event arguments for the button click</param>
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            var addPatientWindow = new AddEditPatientWindow();
            bool? dialogResult = addPatientWindow.ShowDialog();

            if (dialogResult == true)
            {
                
                LoadPatients();
            }
        }

        /// <summary>
        // /// Handles the click event to edit a selected patient by opening the AddEditPatientWindow in "Edit" mode.
        // /// </summary>
        // /// <param name="sender">The button that was clicked</param>
        // /// <param name="e">Event arguments for the button click</param>
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var editPatientWindow = new AddEditPatientWindow(SelectedPatient);
                bool? dialogResult = editPatientWindow.ShowDialog();

                if (dialogResult == true) 
                {
                    LoadPatients();
                }
            }
            else
            {
                MessageBox.Show("Please select a patient to edit.");
            }
        }

        /// <summary>
        // /// Handles the click event to delete the selected patient, after confirming the action.
        // /// </summary>
        // /// <param name="sender">The button that was clicked</param>
        // /// <param name="e">Event arguments for the button click</param>
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPatient != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {SelectedPatient.FName} {SelectedPatient.LName}?",
                    "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _patientDal.DeletePatient(SelectedPatient.PatientId.Value);
                    MessageBox.Show("Patient deleted successfully.");
                    LoadPatients();
                }
            }
            else
            {
                MessageBox.Show("Please select a patient to delete.");
            }
        }

        /// <summary>
        /// Gets the currently selected patient from the DataGrid.
        /// </summary>
        private Patient SelectedPatient
        {
            get
            {
                return (Patient)PatientsDataGrid.SelectedItem;
            }
        }
    }
}
