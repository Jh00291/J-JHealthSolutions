using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J_JHealthSolutions.Model.Other;

namespace J_JHealthSolutions.Model.Domain
{
    public class Test
    {
        public int TestCode { get; set; }
        public string TestName { get; set; }
        public int? LowValue { get; set; }
        public int? HighValue { get; set; }
        public UnitOfMeasure Unit { get; set; }

        public override string ToString()
        {
            return TestName;
        }

    }

}
