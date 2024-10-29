using System;

namespace J_JHealthSolutions.Model
{
    public class Appointment
    {
        public int? AppointmentId { get; set; }
        public int PatientId { get; set; }

        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFullName => $"{PatientFirstName} {PatientLastName}";

        public int DoctorId { get; set; }

        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorFullName => $"{DoctorFirstName} {DoctorLastName}";

        public DateTime DateTime { get; set; }

        private string _reason;
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

        public Status Status { get; set; }

        public Appointment(int patientId, int doctorId, DateTime dateTime, string reason, Status status)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            DateTime = dateTime;
            Reason = reason;
            Status = status;
        }

        public Appointment() { }
    }
}