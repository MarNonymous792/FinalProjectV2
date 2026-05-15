using System;

namespace FinalProjectV2
{
    public class Scholarship
    {
        public int ScholarshipId { get; set; }

        public int ScholarshipID
        {
            get { return ScholarshipId; }
            set { ScholarshipId = value; }
        }

        public int Id
        {
            get { return ScholarshipId; }
            set { ScholarshipId = value; }
        }

        public string Name { get; set; }
        public string Provider { get; set; }
        public string Description { get; set; }
        public int AvailableSlots { get; set; }
        public DateTime Deadline { get; set; }
        public string RequiredCourse { get; set; }
        public int MaxYearLevel { get; set; }
    }
}