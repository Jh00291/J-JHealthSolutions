﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model.Domain
{
    public class VisitReport
    {
        public DateTime VisitDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string NurseName { get; set; }
        public string TestsOrdered { get; set; }
        public string HasAbnormalTests { get; set; }
        public string Diagnosis { get; set; }
        public string PatientLastName { get; set; }
        public string Status { get; set; }
    }
}
