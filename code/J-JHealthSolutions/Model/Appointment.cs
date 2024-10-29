using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public class Appointment
    {
        public int? AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

}
