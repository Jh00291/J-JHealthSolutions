using System;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents an appointment between a patient and a doctor.
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the appointment.
        /// </summary>
        /// <value>
        /// The appointment ID. It is nullable to accommodate appointments that have not yet been assigned an ID.
        /// </value>
        public int? AppointmentId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the patient associated with the appointment.
        /// </summary>
        /// <value>
        /// The patient ID.
        /// </value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        /// The patient's first name.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        /// The patient's last name.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets the full name of the patient by combining the first and last names.
        /// </summary>
        /// <value>
        /// The patient's full name.
        /// </value>
        public string PatientFullName => $"{PatientFirstName} {PatientLastName}";

        /// <summary>
        /// Gets or sets the unique identifier of the doctor associated with the appointment.
        /// </summary>
        /// <value>
        /// The doctor ID.
        /// </value>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the doctor.
        /// </summary>
        /// <value>
        /// The doctor's first name.
        /// </value>
        public string DoctorFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the doctor.
        /// </summary>
        /// <value>
        /// The doctor's last name.
        /// </value>
        public string DoctorLastName { get; set; }

        /// <summary>
        /// Gets the full name of the doctor by combining the first and last names.
        /// </summary>
        /// <value>
        /// The doctor's full name.
        /// </value>
        public string DoctorFullName => $"{DoctorFirstName} {DoctorLastName}";

        /// <summary>
        /// Gets or sets the date and time of the appointment.
        /// </summary>
        /// <value>
        /// The appointment's scheduled date and time.
        /// </value>
        public DateTime DateTime { get; set; }

        private string _reason;

        /// <summary>
        /// Gets or sets the reason for the appointment.
        /// </summary>
        /// <value>
        /// The reason for the appointment. Cannot be null or empty.
        /// </value>
        /// <exception cref="ArgumentException">
        /// Thrown when attempting to set a null or empty value.
        /// </exception>
        public string Reason
        {
            get => _reason;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Reason cannot be null or empty.", nameof(Reason));
                _reason = value;
            }
        }

        /// <summary>
        /// Gets or sets the current status of the appointment.
        /// </summary>
        /// <value>
        /// The status of the appointment, represented by the <see cref="Status"/> enumeration.
        /// </value>
        public Status Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class with specified details.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <param name="dateTime">The date and time of the appointment.</param>
        /// <param name="reason">The reason for the appointment.</param>
        /// <param name="status">The current status of the appointment.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="reason"/> is null or empty.
        /// </exception>
        public Appointment(int patientId, int doctorId, DateTime dateTime, string reason, Status status)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            DateTime = dateTime;
            Reason = reason;
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class.
        /// </summary>
        public Appointment()
        {
        }
    }
}