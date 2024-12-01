using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents a patient in the healthcare system with personal, contact, and status details.
    /// </summary>
    public class Patient
    {
        private int? _patientId;
        private string _fName;
        private string _lName;
        private DateTime _dob;
        private string _address1;
        private string _city;
        private string _state;
        private string _zipcode;
        private string _phone;

        /// <summary>
        /// Patient ID, must be a positive integer.
        /// </summary>
        public int? PatientId
        {
            get => _patientId;
            internal set
            {
                if (value <= 0)
                    throw new ArgumentException("Patient ID must be a positive integer greater than zero.");
                _patientId = value;
            }
        }

        /// <summary>
        /// First name of the patient, cannot be null or empty.
        /// </summary>
        public string FName
        {
            get => _fName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name is required and cannot be empty.");
                _fName = value;
            }
        }

        /// <summary>
        /// Last name of the patient, cannot be null or empty.
        /// </summary>
        public string LName
        {
            get => _lName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name is required and cannot be empty.");
                _lName = value;
            }
        }

        /// <summary>
        /// Date of birth of the patient, must be in the past.
        /// </summary>
        public DateTime DOB
        {
            get => _dob;
            set
            {
                if (value > DateTime.Today)
                    throw new ArgumentException("Date of birth must be in the past. Please enter a valid date in the format MM/dd/yyyy.");
                _dob = value;
            }
        }

        /// <summary>
        /// Address line 1 of the patient, cannot be null or empty.
        /// </summary>
        public string Address1
        {
            get => _address1;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address Line 1 is required and cannot be empty.");
                _address1 = value;
            }
        }

        /// <summary>
        /// Address line 2 of the patient, optional.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City of the patient, cannot be null or empty.
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City is required and cannot be empty.");
                _city = value;
            }
        }

        /// <summary>
        /// State code of the patient, must be a valid US state code.
        /// </summary>
        public string State
        {
            get => _state;
            set
            {
                if (!IsValidState(value))
                    throw new ArgumentException("State code is invalid. /nPlease enter or select a valid two-letter US state code, e.g., 'NY' for New York.");
                _state = value.ToUpper();
            }
        }

        /// <summary>
        /// Zipcode of the patient, must be in the format 12345 or 12345-6789.
        /// </summary>
        public string Zipcode
        {
            get => _zipcode;
            set
            {
                if (!Regex.IsMatch(value, @"^\d{5}(-\d{4})?$"))
                    throw new ArgumentException("Invalid Zipcode format. Use '12345' or '12345-6789'.");
                _zipcode = value;
            }
        }

        /// <summary>
        /// Phone number of the patient, must be a valid 10-digit US number.
        /// </summary>
        public string Phone
        {
            get => _phone;
            set
            {
                if (!Regex.IsMatch(value, @"^\+?1?\d{10}$"))
                    throw new ArgumentException("Invalid phone number format. A valid 10-digit US phone number is required, e.g., '1234567890'.");
                _phone = value;
            }
        }

        public string PatientFullName => $"{FName} {LName}";

        /// <summary>
        /// Gender of the patient, represented as a single character.
        /// </summary>
        public char Gender { get; set; }

        /// <summary>
        /// Indicates whether the patient is active in the system.
        /// </summary>
        public bool Active { get; set; }

        public string PatientDisplayInfo => $"{PatientFullName} | PatientID: {PatientId} | DOB: {DOB:MM/dd/yyyy}";

        /// <summary>
        /// Default constructor for the Patient class.
        /// </summary>
        public Patient()
        {
        }

        /// <summary>
        /// Constructor to initialize a patient with detailed information.
        /// </summary>
        /// <param name="fName">First name of the patient</param>
        /// <param name="lName">Last name of the patient</param>
        /// <param name="dob">Date of birth of the patient</param>
        /// <param name="gender">Gender of the patient</param>
        /// <param name="address1">Address line 1</param>
        /// <param name="address2">Address line 2 (optional)</param>
        /// <param name="city">City of the patient</param>
        /// <param name="state">State code</param>
        /// <param name="zipcode">Zipcode</param>
        /// <param name="phone">Phone number of the patient</param>
        /// <param name="active">Indicates if the patient is active</param>
        public Patient(string fName, string lName, DateTime dob, string address1, char gender,
            string address2, string city, string state, string zipcode, string phone, bool active)
        {
            FName = fName;
            LName = lName;
            DOB = dob;
            Gender = gender;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zipcode = zipcode;
            Phone = phone;
            Active = active;
        }

        /// <summary>
        /// Validates whether the provided state code is a valid US state.
        /// </summary>
        /// <param name="state">State code to validate</param>
        /// <returns>True if the state code is valid, otherwise false</returns>
        private bool IsValidState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                return false;

            string[] states = {
                "AL","AK","AZ","AR","CA","CO","CT","DE","FL","GA","HI","ID","IL","IN",
                "IA","KS","KY","LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV",
                "NH","NJ","NM","NY","NC","ND","OH","OK","OR","PA","RI","SC","SD","TN",
                "TX","UT","VT","VA","WA","WV","WI","WY"
            };
            return Array.Exists(states, s => s.Equals(state, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return PatientDisplayInfo;
        }
    }
}
