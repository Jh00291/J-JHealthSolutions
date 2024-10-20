using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace J_JHealthSolutions.Views
{
    public partial class AddPatientWindow : Window
    {
        private PatientDal _patientDal = new PatientDal();

        public AddPatientWindow()
        {
            InitializeComponent();
        }

        private void SavePatient_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                var newPatient = new Patient
                {
                    FName = firstNameTextBox.Text,
                    LName = lastNameTextBox.Text,
                    Dob = dobDatePicker.SelectedDate.Value,
                    Gender = genderComboBox.Text[0],
                    Address1 = address1TextBox.Text,
                    Address2 = address2TextBox.Text,
                    City = cityTextBox.Text,
                    State = stateComboBox.Text,
                    Zipcode = zipcodeTextBox.Text,
                    Phone = phoneTextBox.Text,
                    Active = activeCheckBox.IsChecked.Value
                };

                _patientDal.AddPatient(newPatient);
                MessageBox.Show("Patient added successfully.");
                this.Close();  // Close the AddPatient window after saving
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) || string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                MessageBox.Show("First Name and Last Name are required.");
                return false;
            }

            if (dobDatePicker.SelectedDate == null || dobDatePicker.SelectedDate >= DateTime.Today)
            {
                MessageBox.Show("Please select a valid Date of Birth.");
                return false;
            }

            if (!Regex.IsMatch(zipcodeTextBox.Text, @"^\d{5}(?:[-\s]\d{4})?$"))
            {
                MessageBox.Show("Invalid Zip Code format.");
                return false;
            }

            if (!Regex.IsMatch(phoneTextBox.Text, @"^\+?1?\d{10}$"))
            {
                MessageBox.Show("Invalid Phone Number format.");
                return false;
            }

            return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
