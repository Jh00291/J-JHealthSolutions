using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    /// <summary>
    /// Represents the possible statuses of an appointment.
    /// </summary>
    public enum Status
    {
        Scheduled,
        Completed,
        Cancelled,
        NoShow,
        InProgress
    }
}
