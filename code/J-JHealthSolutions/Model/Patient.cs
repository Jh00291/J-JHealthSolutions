using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public class Patient
    {
        public int PatientId { get; set; }     
        public string FName { get; set; }        
        public string LName { get; set; }      
        public DateTime Dob { get; set; }      
        public string Address1 { get; set; }   
        public string Address2 { get; set; }   
        public string City { get; set; }       
        public string State { get; set; }         
        public string Zipcode { get; set; }
        public string Phone { get; set; }           
        public bool Active { get; set; }        

        public Patient()
        {
        }

        public Patient(int patientId, string fName, string lName, DateTime dob, string address1,
            string address2, string city, string state, string zipcode, string phone, bool active)
        {
            PatientId = patientId;
            FName = fName;
            LName = lName;
            Dob = dob;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zipcode = zipcode;
            Phone = phone;
            Active = active;
        }
    }
}
