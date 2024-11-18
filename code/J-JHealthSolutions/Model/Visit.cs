using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents a visit in the healthcare system, including check-up details and diagnoses.
    /// </summary>
    public class Visit
    {
        private int? _visitId;
        private DateTime _visitDateTime;

        /// <summary>
        /// Visit ID, must be a positive integer.
        /// </summary>
        public int? VisitId
        {
            get => _visitId;
            internal set
            {
                if (value <= 0)
                    throw new ArgumentException("Visit ID must be a positive integer greater than zero.");
                _visitId = value;
            }
        }

        /// <summary>
        /// Appointment ID, must be a positive integer and is required.
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// Patient ID, must be a positive integer and is required.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Doctor ID, must be a positive integer and is required.
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Nurse ID, must be a positive integer and is required.
        /// </summary>
        public int NurseId { get; set; }

        /// <summary>
        /// Visit date and time, must be in the past or present.
        /// </summary>
        public DateTime VisitDateTime
        {
            get => _visitDateTime;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Visit date and time cannot be in the future.");
                _visitDateTime = value;
            }
        }

        /// <summary>
        /// Weight in kilograms, nullable.
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Height in centimeters, nullable.
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Systolic blood pressure, nullable.
        /// </summary>
        public int? BloodPressureSystolic { get; set; }

        /// <summary>
        /// Diastolic blood pressure, nullable.
        /// </summary>
        public int? BloodPressureDiastolic { get; set; }

        /// <summary>
        /// Body temperature in Celsius, nullable.
        /// </summary>
        public decimal? Temperature { get; set; }

        /// <summary>
        /// Pulse rate, nullable.
        /// </summary>
        public int? Pulse { get; set; }

        /// <summary>
        /// Symptoms described during the visit, nullable.
        /// </summary>
        public string Symptoms { get; set; }

        /// <summary>
        /// Initial diagnosis at the time of visit, nullable.
        /// </summary>
        public string InitialDiagnosis { get; set; }

        /// <summary>
        /// Final diagnosis, nullable.
        /// </summary>
        public string FinalDiagnosis { get; set; }

        /// <summary>
        /// Full name of the patient.
        /// </summary>
        public string PatientFullName { get; set; }

        /// <summary>
        /// Full name of the doctor.
        /// </summary>
        public string DoctorFullName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the nurse.
        /// </summary>
        /// <value>
        /// The full name of the nurse.
        /// </value>
        public string NurseFullName { get; set; }

        /// <summary>
        /// Gets or sets the patient dob.
        /// </summary>
        /// <value>
        /// The patient dob.
        /// </value>
        public DateTime PatientDOB { get; set; }

        /// <summary>
        /// Status of the visit, cannot be null or empty.
        /// </summary>
        private string _visitStatus;
        public string VisitStatus
        {
            get => _visitStatus;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Visit status is required and cannot be empty.");
                _visitStatus = value;
            }
        }

        /// <summary>
        /// Default constructor for the Visit class.
        /// </summary>
        public Visit()
        {
        }

        /// <summary>
        /// Constructor to initialize a visit with details.
        /// </summary>
        /// <param name="appointmentId">ID of the associated appointment</param>
        /// <param name="patientId">ID of the patient</param>
        /// <param name="doctorId">ID of the doctor</param>
        /// <param name="nurseId">ID of the nurse</param>
        /// <param name="visitDateTime">Date and time of the visit</param>
        /// <param name="visitStatus">Status of the visit</param>
        public Visit(int appointmentId, int patientId, int doctorId, int nurseId, DateTime visitDateTime, string visitStatus)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            DoctorId = doctorId;
            NurseId = nurseId;
            VisitDateTime = visitDateTime;
            VisitStatus = visitStatus;
        }
    }
}
