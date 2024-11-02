using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public class Doctor : Employee
    {
        public int DoctorId { get; set; }

        public Doctor(int? userId, string fName, string lName, DateTime dob, char gender,
            string address1, string address2, string city, string state, string zipcode, string personalPhone, int doctorId)
            : base(userId, fName, lName, dob, gender, address1, address2, city, state, zipcode, personalPhone)
        {
            DoctorId = doctorId;
        }
    }
}
