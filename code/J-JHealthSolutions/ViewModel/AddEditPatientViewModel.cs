using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Google.Protobuf.WellKnownTypes;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using Org.BouncyCastle.Asn1.Cms;

namespace J_JHealthSolutions.ViewModels
{
    public class AddEditPatientViewModel : INotifyPropertyChanged
    {
        // Event for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to raise PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Patient Model
        private Patient _patient;

        // Properties bound to UI elements
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        private DateTime? _dob;
        public DateTime? DOB
        {
            get => _dob;
            set { _dob = value; OnPropertyChanged(nameof(DOB)); }
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        private string _address1;
        public string Address1
        {
            get => _address1;
            set { _address1 = value; OnPropertyChanged(nameof(Address1)); }
        }

        private string _address2;
        public string Address2
        {
            get => _address2;
            set { _address2 = value; OnPropertyChanged(nameof(Address2)); }
        }

        private string _city;
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(nameof(City)); }
        }

        private string _state;
        public string State
        {
            get => _state;
            set { _state = value; OnPropertyChanged(nameof(State)); }
        }

        private string _zipcode;
        public string Zipcode
        {
            get => _zipcode;
            set { _zipcode = value; OnPropertyChanged(nameof(Zipcode)); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

        private bool _active;
        public bool Active
        {
            get => _active;
            set { _active = value; OnPropertyChanged(nameof(Active)); }
        }

        // Error message properties
        private string _firstNameErrorMessage;
        public string FirstNameErrorMessage
        {
            get => _firstNameErrorMessage;
            set { _firstNameErrorMessage = value; OnPropertyChanged(nameof(FirstNameErrorMessage)); }
        }

        private string _lastNameErrorMessage;
        public string LastNameErrorMessage
        {
            get => _lastNameErrorMessage;
            set { _lastNameErrorMessage = value; OnPropertyChanged(nameof(LastNameErrorMessage)); }
        }

        private string _dobErrorMessage;
        public string DOBErrorMessage
        {
            get => _dobErrorMessage;
            set { _dobErrorMessage = value; OnPropertyChanged(nameof(DOBErrorMessage)); }
        }

        private string _genderErrorMessage;
        public string GenderErrorMessage
        {
            get => _genderErrorMessage;
            set { _genderErrorMessage = value; OnPropertyChanged(nameof(GenderErrorMessage)); }
        }

        private string _stateErrorMessage;
        public string StateErrorMessage
        {
            get => _stateErrorMessage;
            set { _stateErrorMessage = value; OnPropertyChanged(nameof(StateErrorMessage)); }
        }

        private string _address1ErrorMessage;
        public string Address1ErrorMessage
        {
            get => _address1ErrorMessage;
            set { _address1ErrorMessage = value; OnPropertyChanged(nameof(Address1ErrorMessage)); }
        }

        private string _cityErrorMessage;
        public string CityErrorMessage
        {
            get => _cityErrorMessage;
            set { _cityErrorMessage = value; OnPropertyChanged(nameof(CityErrorMessage)); }
        }

        private string _zipcodeErrorMessage;
        public string ZipcodeErrorMessage
        {
            get => _zipcodeErrorMessage;
            set { _zipcodeErrorMessage = value; OnPropertyChanged(nameof(ZipcodeErrorMessage)); }
        }

        private string _phoneErrorMessage;
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set { _phoneErrorMessage = value; OnPropertyChanged(nameof(PhoneErrorMessage)); }
        }

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Action to close the window
        public Action<bool?> CloseAction { get; set; }

        // Constructor for Adding a new Patient
        public AddEditPatientViewModel()
        {
            _patient = new Patient();
            SaveCommand = new RelayCommand(SavePatient);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Constructor for Editing an existing Patient
        public AddEditPatientViewModel(Patient patient)
        {
            _patient = patient;

            // Populate properties with existing patient data
            FirstName = _patient.FName;
            LastName = _patient.LName;
            DOB = _patient.DOB;
            Gender = _patient.Gender.ToString();
            Address1 = _patient.Address1;
            Address2 = _patient.Address2;
            City = _patient.City;
            State = _patient.State;
            Zipcode = _patient.Zipcode;
            Phone = _patient.Phone;
            Active = _patient.Active;

            SaveCommand = new RelayCommand(SavePatient);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Method to save patient data
        private void SavePatient(object parameter)
        {
            bool hasError = false;

            // Reset all error messages
            FirstNameErrorMessage = string.Empty;
            LastNameErrorMessage = string.Empty;
            DOBErrorMessage = string.Empty;
            GenderErrorMessage = string.Empty;
            StateErrorMessage = string.Empty;
            Address1ErrorMessage = string.Empty;
            CityErrorMessage = string.Empty;
            ZipcodeErrorMessage = string.Empty;
            PhoneErrorMessage = string.Empty;

            // Validate each field and set the respective error message if needed
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameErrorMessage = "First name is required.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameErrorMessage = "Last name is required.";
                hasError = true;
            }

            if (DOB == null)
            {
                DOBErrorMessage = "Date of birth is required. Example: 01/25/2005";
                hasError = true;
            } else if (DOB > DateTime.Today)
            {
                DOBErrorMessage = "Date of birth cannot be in the future.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Gender))
            {
                GenderErrorMessage = "Gender is required.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(State))
            {
                StateErrorMessage = "State is required.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Address1))
            {
                Address1ErrorMessage = "Address1 is required.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(City))
            {
                CityErrorMessage = "City is required.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Zipcode))
            {
                ZipcodeErrorMessage = "Zipcode is required. Example: 12345";
                hasError = true;
            } else if (!Regex.IsMatch(Zipcode, @"^\d{5}$"))
            {
                ZipcodeErrorMessage = "A valid 5-digit US zipcode is required, e.g., '12345'.";
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                PhoneErrorMessage = "Phone is required. Example: 0123456789";
                hasError = true;
            } else if (!Regex.IsMatch(Phone, @"^\+?1?\d{10}$"))
            {
                PhoneErrorMessage = "A valid 10-digit US phone number is required, e.g., '1234567890'.";
                hasError = true;
            }

            if (hasError)
            {
                return;
            }

            try
            {
                // Populate the patient model
                _patient.FName = FirstName;
                _patient.LName = LastName;
                _patient.DOB = DOB.Value;
                _patient.Gender = Gender[0];
                _patient.Address1 = Address1;
                _patient.Address2 = string.IsNullOrWhiteSpace(Address2) ? null : Address2;
                _patient.City = City;
                _patient.State = State;
                _patient.Zipcode = Zipcode;
                _patient.Phone = Phone;
                _patient.Active = Active;

                // Add or update the patient
                if (_patient.PatientId == null)
                {
                    // Add new patient
                    int generatedId = PatientDal.AddPatient(_patient);
                }
                else
                {
                    // Update existing patient
                    bool success = PatientDal.UpdatePatient(_patient);
                    if (!success)
                    {
                        ShowErrorDialog("Error", "Failed to update patient.");
                        return;
                    }
                }

                // Close the window if operation is successful
                CloseAction?.Invoke(true);
            }
            catch (Exception ex)
            {
                // Show detailed error message
                ShowErrorDialog("Error Saving Patient", $"An error occurred: {ex.Message}");
            }
        }

        // Method to cancel the operation
        private void Cancel(object parameter)
        {
            CloseAction?.Invoke(false);
        }

        // Method to show error dialog
        private void ShowErrorDialog(string title, string message)
        {
            var errorDialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(10) },
                Width = 550,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            errorDialog.ShowDialog();
        }
    }
}
