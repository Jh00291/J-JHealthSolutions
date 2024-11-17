using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public class TestOrder
    {

        private int? _testOrderID;
        public int VisitId { get; set; }
        public int TestCode { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? PerformedDateTime { get; set; }
        public int Result { get; set; }
        public bool Abnormal { get; set; }
        public Test Test { get; set; }

        public int? TestOrderID
        {
            get => _testOrderID;
            set
            {

            
                if (value <= 0)
                    throw new ArgumentException("Test Order ID must be a positive integer greater than zero.");
                _testOrderID = value;
            }
        }

        public TestOrder(int? testOrderID, int visitID, int testCode, DateTime orderDateTime, DateTime? performedDateTime, int result, bool abnormal, Test test)
        {
            this.TestOrderID = testOrderID;
            this.VisitId = visitID;
            this.TestCode = testCode;
            this.OrderDateTime = orderDateTime;
            this.PerformedDateTime = performedDateTime;
            this.Result = result;
            this.Abnormal = abnormal;
            Test = test;
        }

        public TestOrder() { }


    }

}
