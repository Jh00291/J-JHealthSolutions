using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public class Nurse : Employee
    {
        public int NurseId { get; set; }

        public Nurse(int? userId, string fName, string lName, DateTime dob, char gender,
            string address1, string address2, string city, string state, string zipcode, string personalPhone, int nurseId)
            : base(userId, fName, lName, dob, gender, address1, address2, city, state, zipcode, personalPhone)
        {
            NurseId = nurseId;
        }

        public override String ToString()
        {
            return $"{EmployeeFullName} | NurseID:{NurseId}";
        }
    }
}
