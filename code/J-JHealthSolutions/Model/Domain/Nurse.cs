using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model.Domain
{
    /// <summary>
    /// Represents a nurse, inheriting from the <see cref="Employee"/> class.
    /// </summary>
    public class Nurse : Employee
    {
        /// <summary>
        /// Gets or sets the unique identifier for the nurse.
        /// </summary>
        /// <value>
        /// The nurse ID.
        /// </value>
        public int NurseId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nurse"/> class with specified details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user associated with the nurse.</param>
        /// <param name="fName">The first name of the nurse.</param>
        /// <param name="lName">The last name of the nurse.</param>
        /// <param name="dob">The date of birth of the nurse.</param>
        /// <param name="gender">The gender of the nurse, represented as a single character.</param>
        /// <param name="address1">The primary address of the nurse.</param>
        /// <param name="address2">The secondary address of the nurse. Can be null or empty.</param>
        /// <param name="city">The city where the nurse resides.</param>
        /// <param name="state">The state where the nurse resides.</param>
        /// <param name="zipcode">The ZIP code of the nurse's residence.</param>
        /// <param name="personalPhone">The personal phone number of the nurse.</param>
        /// <param name="nurseId">The unique identifier assigned to the nurse.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the required string parameters are null or empty, or when <paramref name="gender"/> is not a valid character.
        /// </exception>
        public Nurse(int? userId, string fName, string lName, DateTime dob, char gender,
            string address1, string address2, string city, string state, string zipcode, string personalPhone, int nurseId)
            : base(userId, fName, lName, dob, gender, address1, address2, city, state, zipcode, personalPhone)
        {
            NurseId = nurseId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nurse"/> class with default values.
        /// </summary>
        public Nurse() { }

        /// <summary>
        /// Returns a string that represents the current nurse.
        /// </summary>
        /// <returns>
        /// A string that includes the full name of the nurse and their Nurse ID.
        /// </returns>
        public override string ToString()
        {
            return $"{EmployeeFullName} | NurseID:{NurseId}";
        }
    }
}
