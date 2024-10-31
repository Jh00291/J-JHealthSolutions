using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// A window for adding or editing a patient's information.
    /// </summary>
    public partial class AddEditPatientWindow : Window
    {
        private PatientDal _patientDal = new PatientDal();
        private Patient _patient;

        /// <summary>
        /// Initializes a new instance of the AddEditPatientWindow for adding a new patient.
        /// </summary>
        public AddEditPatientWindow()
        {
            InitializeComponent();
            this.Title = "Add Patient";
            this.titleLabel.Content = "Add Patient";
            _patient = new Patient();
        }

        /// <summary>
        /// Initializes a new instance of the AddEditPatientWindow for editing an existing patient.
        /// </summary>
        /// <param name="patient">The patient to be edited</param>
        public AddEditPatientWindow(Patient patient)
        {
            InitializeComponent();
            this.Title = "Edit Patient: " + patient.FName + " " + patient.LName;
            this.titleLabel.Content = "Edit Patient: " + patient.FName + " " + patient.LName;
            _patient = patient;
            PopulatePatientData(patient);
        }

        /// <summary>
        /// Populates the window fields with the existing patient's data.
        /// </summary>
        /// <param name="patient">The patient whose data is used to populate the fields</param>
        private void PopulatePatientData(Patient patient)
        {
            firstNameTextBox.Text = patient.FName;
            lastNameTextBox.Text = patient.LName;
            dobDatePicker.SelectedDate = patient.DOB;
            genderComboBox.SelectedItem = genderComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == patient.Gender.ToString());
            address1TextBox.Text = patient.Address1;
            address2TextBox.Text = patient.Address2;
            cityTextBox.Text = patient.City;
            stateComboBox.SelectedItem = stateComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == patient.State);
            zipcodeTextBox.Text = patient.Zipcode;
            phoneTextBox.Text = patient.Phone;
            activeCheckBox.IsChecked = patient.Active;
        }

        /// <summary>
        /// Saves the patient data, either adding a new patient or updating an existing one.
        /// </summary>
        private void SavePatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate required fields before accessing them
                if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
                    throw new ArgumentException("First name is required.");
                if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
                    throw new ArgumentException("Last name is required.");
                if (dobDatePicker.SelectedDate == null)
                    throw new ArgumentException("Date of birth is required. Please select a date.");
                if (genderComboBox.SelectedItem == null)
                    throw new ArgumentException("Gender is required. Please select an option.");
                if (string.IsNullOrWhiteSpace(stateComboBox.Text))
                    throw new ArgumentException("State is required. Please select a state.");

                if (_patient == null || _patient.PatientId == null)
                {
                    // If no patient exists, create a new one
                    var newPatient = new Patient
                    {
                        FName = firstNameTextBox.Text,
                        LName = lastNameTextBox.Text,
                        DOB = dobDatePicker.SelectedDate.Value,
                        Gender = genderComboBox.Text[0],
                        Address1 = address1TextBox.Text,
                        Address2 = address2TextBox.Text,
                        City = cityTextBox.Text,
                        State = stateComboBox.Text,
                        Zipcode = zipcodeTextBox.Text,
                        Phone = phoneTextBox.Text,
                        Active = activeCheckBox.IsChecked ?? false
                    };

                    _patientDal.AddPatient(newPatient);
                    ShowSuccessDialog("Patient added successfully.");
                }
                else
                {
                    // Update existing patient
                    _patient.FName = firstNameTextBox.Text;
                    _patient.LName = lastNameTextBox.Text;
                    _patient.DOB = dobDatePicker.SelectedDate.Value;
                    _patient.Gender = !string.IsNullOrEmpty(genderComboBox.Text) ? genderComboBox.Text[0] : 'U';
                    _patient.Address1 = address1TextBox.Text;
                    _patient.Address2 = address2TextBox.Text;
                    _patient.City = cityTextBox.Text;
                    _patient.State = stateComboBox.Text;
                    _patient.Zipcode = zipcodeTextBox.Text;
                    _patient.Phone = phoneTextBox.Text;
                    _patient.Active = activeCheckBox.IsChecked.Value;

                    _patientDal.UpdatePatient(_patient); 
                    ShowSuccessDialog("Patient updated successfully.");
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (ArgumentException ex)
            {
                ShowErrorDialog("Validation Error", ex.Message);
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Error", ex.Message);
            }
        }

        /// <summary>
        // /// Shows a custom success dialog with a message.
        // /// </summary>
        // /// <param name="message">The message to display</param>
        private void ShowSuccessDialog(string message)
        {
            var successDialog = new Window
            {
                Title = "Success",
                Content = new TextBlock { Text = message, Margin = new Thickness(10) },
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            successDialog.ShowDialog();
        }

        /// <summary>
        // /// Shows a custom error dialog with a title and message.
        // /// </summary>
        // /// <param name="title">The title of the error dialog</param>
        // /// <param name="message">The error message to display</param>
        private void ShowErrorDialog(string title, string message)
        {
            var errorDialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(10) },
                Width =550,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            errorDialog.ShowDialog();
        }

        /// <summary>
        /// Closes the window when the cancel button is clicked.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
