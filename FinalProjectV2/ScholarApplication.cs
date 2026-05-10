using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    internal class ScholarApplication
    {
        public int ApplicationID { get; set; }
        public string UserID { get; set; }
        public int ScholarshipID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public string ScholarshipName { get; set; }
        public string Provider { get; set; }
    }
}
