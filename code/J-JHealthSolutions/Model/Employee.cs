using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents an employee in the system, with personal and contact details.
    /// </summary>
    public class Employee
    {
        private int? _employeeId;
        private string _fName;
        private string _lName;
        private DateTime _dob;
        private string _address1;
        private string _city;
        private string _state;
        private string _zipcode;
        private string _personalPhone;

        /// <summary>
        /// Employee ID, must be a positive integer.
        /// </summary>
        internal int? EmployeeId
        {
            get => _employeeId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("EmployeeId must be a positive integer.");
                _employeeId = value;
            }
        }

        /// <summary>
        /// The User ID associated with the employee.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// First name of the employee, cannot be null or empty.
        /// </summary>
        public string FName
        {
            get => _fName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be null or empty.");
                _fName = value;
            }
        }

        /// <summary>
        /// Last name of the employee, cannot be null or empty.
        /// </summary>
        public string LName
        {
            get => _lName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be null or empty.");
                _lName = value;
            }
        }

        /// <summary>
        /// Date of birth of the employee, must be in the past.
        /// </summary>
        public DateTime Dob
        {
            get => _dob;
            set
            {
                if (value >= DateTime.Today)
                    throw new ArgumentException("Date of birth must be in the past.");
                _dob = value;
            }
        }

        /// <summary>
        /// Address line 1 of the employee, cannot be null or empty.
        /// </summary>
        public string Address1
        {
            get => _address1;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address1 cannot be null or empty.");
                _address1 = value;
            }
        }

        /// <summary>
        /// Address line 2 of the employee, optional.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// City of the employee, cannot be null or empty.
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("City cannot be null or empty.");
                _city = value;
            }
        }

        /// <summary>
        /// State code of the employee, must be a valid US state code.
        /// </summary>
        public string State
        {
            get => _state;
            set
            {
                if (!IsValidState(value))
                    throw new ArgumentException("Invalid state code.");
                _state = value.ToUpper();
            }
        }

        /// <summary>
        /// Zipcode of the employee, must be in the format 12345 or 12345-6789.
        /// </summary>
        public string Zipcode
        {
            get => _zipcode;
            set
            {
                if (!Regex.IsMatch(value, @"^\d{5}(-\d{4})?$"))
                    throw new ArgumentException("Invalid zipcode format.");
                _zipcode = value;
            }
        }

        /// <summary>
        /// Personal phone number of the employee, must be a valid 10-digit US number.
        /// </summary>
        public string PersonalPhone
        {
            get => _personalPhone;
            set
            {
                if (!Regex.IsMatch(value, @"^\+?1?\d{10}$"))
                    throw new ArgumentException("Invalid phone number format.");
                _personalPhone = value;
            }
        }

        public string EmployeeFullName => $"{FName} {LName}";

        /// <summary>
        /// Gender of the employee, represented as a single character.
        /// </summary>
        public char Gender { get; set; }

        /// <summary>
        /// Default constructor for the Employee class.
        /// </summary>
        public Employee()
        {
        }

        /// <summary>
        /// Constructor to initialize an employee with detailed information.
        /// </summary>
        /// <param name="userId">User ID associated with the employee</param>
        /// <param name="fName">First name</param>
        /// <param name="lName">Last name</param>
        /// <param name="dob">Date of birth</param>
        /// <param name="gender">Gender</param>
        /// <param name="address1">Address line 1</param>
        /// <param name="address2">Address line 2</param>
        /// <param name="city">City</param>
        /// <param name="state">State code</param>
        /// <param name="zipcode">Zipcode</param>
        /// <param name="personalPhone">Personal phone number</param>
        public Employee(int? userId, string fName, string lName, DateTime dob, char gender,
            string address1, string address2, string city, string state, string zipcode, string personalPhone)
        {
            UserId = userId;
            FName = fName;
            LName = lName;
            Dob = dob;
            Gender = gender;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zipcode = zipcode;
            PersonalPhone = personalPhone;
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
            return EmployeeFullName;
        }
    }
}
