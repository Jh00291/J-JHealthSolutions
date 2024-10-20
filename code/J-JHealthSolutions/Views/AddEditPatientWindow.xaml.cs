using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditPatientWindow : Window
    {
        private PatientDal _patientDal = new PatientDal();
        private Patient _patient;

        public AddEditPatientWindow()
        {
            InitializeComponent();
            _patient = new Patient();
        }

        public AddEditPatientWindow(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            PopulatePatientData(patient);
        }

        private void PopulatePatientData(Patient patient)
        {
            firstNameTextBox.Text = patient.FName;
            lastNameTextBox.Text = patient.LName;
            dobDatePicker.SelectedDate = patient.DOB;
            genderComboBox.SelectedValue = patient.Gender.ToString();
            address1TextBox.Text = patient.Address1;
            address2TextBox.Text = patient.Address2;
            cityTextBox.Text = patient.City;
            stateComboBox.SelectedValue = patient.State;
            zipcodeTextBox.Text = patient.Zipcode;
            phoneTextBox.Text = patient.Phone;
            activeCheckBox.IsChecked = patient.Active;
        }

        private void SavePatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                    _patientDal.AddPatient(newPatient);  // Add the new patient
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

                    _patientDal.UpdatePatient(_patient);  // Update the patient
                    ShowSuccessDialog("Patient updated successfully.");
                }

                // Close the dialog and return true as the result
                this.DialogResult = true;
                this.Close();  // Close the window after successful save
            }
            catch (ArgumentException ex)
            {
                // Catch validation exceptions from the Patient class
                ShowErrorDialog("Validation Error", ex.Message);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                ShowErrorDialog("Error", ex.Message);
            }
        }

        // Custom success dialog
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

        // Custom error dialog for validation or other errors
        private void ShowErrorDialog(string title, string message)
        {
            var errorDialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(10) },
                Width = 350,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            errorDialog.ShowDialog();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
