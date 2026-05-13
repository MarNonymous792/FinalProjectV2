using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    public class Scholarship
    {
        public int ScholarshipID { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Description { get; set; }

        public string RequiredCourse { get; set; }

        public int MaxYearLevel { get; set; }

        public DateTime Deadline { get; set; }
        public int AvailableSlots { get; set; }

    }
}