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
            bool hasError = false;

            // Reset all error labels
            firstNameErrorLabel.Visibility = Visibility.Collapsed;
            lastNameErrorLabel.Visibility = Visibility.Collapsed;
            dobErrorLabel.Visibility = Visibility.Collapsed;
            genderErrorLabel.Visibility = Visibility.Collapsed;
            stateErrorLabel.Visibility = Visibility.Collapsed;
            address1ErrorLabel.Visibility = Visibility.Collapsed;
            cityErrorLabel.Visibility = Visibility.Collapsed;
            zipcodeErrorLabel.Visibility = Visibility.Collapsed;
            phoneErrorLabel.Visibility = Visibility.Collapsed;

            // Validate each field and show the respective error label if needed
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                firstNameErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                lastNameErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (dobDatePicker.SelectedDate == null)
            {
                dobErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (genderComboBox.SelectedItem == null)
            {
                genderErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(stateComboBox.Text))
            {
                stateErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(address1TextBox.Text))
            {
                address1ErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(cityTextBox.Text))
            {
                cityErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(zipcodeTextBox.Text))
            {
                zipcodeErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                phoneErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            // If there are errors, stop here
            if (hasError)
            {
                return;
            }

            // Proceed with saving the patient data
            if (_patient == null || _patient.PatientId == null)
            {
                // Add new patient
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
