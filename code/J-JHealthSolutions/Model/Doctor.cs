namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents a doctor, inheriting from the <see cref="Employee"/> class.
    /// </summary>
    public class Doctor : Employee
    {
        /// <summary>
        /// Gets or sets the unique identifier for the doctor.
        /// </summary>
        /// <value>
        /// The doctor ID.
        /// </value>
        public int DoctorId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class with specified details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user associated with the doctor.</param>
        /// <param name="fName">The first name of the doctor.</param>
        /// <param name="lName">The last name of the doctor.</param>
        /// <param name="dob">The date of birth of the doctor.</param>
        /// <param name="gender">The gender of the doctor, represented as a single character.</param>
        /// <param name="address1">The primary address of the doctor.</param>
        /// <param name="address2">The secondary address of the doctor. Can be null or empty.</param>
        /// <param name="city">The city where the doctor resides.</param>
        /// <param name="state">The state where the doctor resides.</param>
        /// <param name="zipcode">The ZIP code of the doctor's residence.</param>
        /// <param name="personalPhone">The personal phone number of the doctor.</param>
        /// <param name="doctorId">The unique identifier assigned to the doctor.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when any of the required string parameters are null or empty, or when <paramref name="gender"/> is not a valid character.
        /// </exception>
        public Doctor(int? userId, string fName, string lName, DateTime dob, char gender,
            string address1, string address2, string city, string state, string zipcode, string personalPhone, int doctorId)
            : base(userId, fName, lName, dob, gender, address1, address2, city, state, zipcode, personalPhone)
        {
            DoctorId = doctorId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class with default values.
        /// </summary>
        public Doctor() { }

        /// <summary>
        /// Returns a string that represents the current doctor.
        /// </summary>
        /// <returns>
        /// A string that includes the full name of the doctor and their Doctor ID.
        /// </returns>
        public override string ToString()
        {
            return $"{EmployeeFullName} | DoctorID:{DoctorId}";
        }
    }
}
