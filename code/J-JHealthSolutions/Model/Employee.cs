using System;
using System.Text.RegularExpressions;

namespace J_JHealthSolutions.Model
{
    public class Employee
    {
        private int _employeeId;
        private string _fName;
        private string _lName;
        private DateTime _dob;
        private string _address1;
        private string _city;
        private string _state;
        private string _zipcode;
        private string _personalPhone;

        public int EmployeeId
        {
            get => _employeeId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("EmployeeId must be a positive integer.");
                _employeeId = value;
            }
        }

        public int? UserId { get; set; }

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

        public string Address2 { get; set; }

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

        public Employee()
        {
        }

        public Employee(int employeeId, int? userId, string fName, string lName, DateTime dob,
            string address1, string address2, string city, string state, string zipcode, string personalPhone)
        {
            EmployeeId = employeeId;
            UserId = userId;
            FName = fName;
            LName = lName;
            Dob = dob;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zipcode = zipcode;
            PersonalPhone = personalPhone;
        }

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
    }
}
