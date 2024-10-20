using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditPatientWindow : Window
    {
        private PatientDal _patientDal = new PatientDal();
        private Patient _patient;

        public AddEditPatientWindow()
        {
            InitializeComponent();
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
            if (ValidateInputs())
            {
                if (_patient == null)
                {
                    // If no patient was passed, it's a new patient (Add)
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
                        Active = activeCheckBox.IsChecked.Value
                    };

                    _patientDal.AddPatient(newPatient);  // Add the new patient
                    MessageBox.Show("Patient added successfully.");
                }
                else
                {
                    // If a patient was passed, it's an existing patient (Edit)
                    _patient.FName = firstNameTextBox.Text;
                    _patient.LName = lastNameTextBox.Text;
                    _patient.DOB = dobDatePicker.SelectedDate.Value;
                    _patient.Gender = genderComboBox.Text[0];
                    _patient.Address1 = address1TextBox.Text;
                    _patient.Address2 = address2TextBox.Text;
                    _patient.City = cityTextBox.Text;
                    _patient.State = stateComboBox.Text;
                    _patient.Zipcode = zipcodeTextBox.Text;
                    _patient.Phone = phoneTextBox.Text;
                    _patient.Active = activeCheckBox.IsChecked.Value;

                    _patientDal.UpdatePatient(_patient);  // Update the existing patient
                    MessageBox.Show("Patient updated successfully.");
                }

                this.Close();  // Close the window after saving
            }
        }


        private bool ValidateInputs()
        {
            // Check if first name is provided
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                MessageBox.Show("First Name is required.");
                return false;
            }

            // Check if last name is provided
            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                MessageBox.Show("Last Name is required.");
                return false;
            }

            // Check if date of birth is provided and valid
            if (dobDatePicker.SelectedDate == null || dobDatePicker.SelectedDate >= DateTime.Today)
            {
                MessageBox.Show("Please select a valid Date of Birth.");
                return false;
            }

            // Check if gender is selected
            if (genderComboBox.SelectedItem == null)
            {
                MessageBox.Show("Gender is required.");
                return false;
            }

            // Check if address line 1 is provided
            if (string.IsNullOrWhiteSpace(address1TextBox.Text))
            {
                MessageBox.Show("Address 1 is required.");
                return false;
            }

            // Check if city is provided
            if (string.IsNullOrWhiteSpace(cityTextBox.Text))
            {
                MessageBox.Show("City is required.");
                return false;
            }

            // Check if state is selected
            if (stateComboBox.SelectedItem == null)
            {
                MessageBox.Show("State is required.");
                return false;
            }

            // Check if zip code is valid (US format)
            if (!Regex.IsMatch(zipcodeTextBox.Text, @"^\d{5}(?:[-\s]\d{4})?$"))
            {
                MessageBox.Show("Invalid Zip Code format.");
                return false;
            }

            // Check if phone number is valid (10 digits, optional +1 for country code)
            if (!Regex.IsMatch(phoneTextBox.Text, @"^\+?1?\d{10}$"))
            {
                MessageBox.Show("Invalid Phone Number format.");
                return false;
            }

            // All checks passed
            return true;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
