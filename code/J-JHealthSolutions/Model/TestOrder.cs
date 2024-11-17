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

        public int? TestOrderID
        {
            get => _testOrderID;
            internal set
            {

            
                if (value <= 0)
                    throw new ArgumentException("Test Order ID must be a positive integer greater than zero.");
                _testOrderID = value;
            }
        }
        Test Test { get; set; }

    }

}
