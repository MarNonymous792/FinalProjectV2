using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    public class ScholarApplication
    {
        public int ApplicationId { get; set; }
        public int ScholarshipId { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string ApplicationLetterOrCvPath { get; set; }
        public string StudyLoadPath { get; set; }
        public string GoodMoralPath { get; set; }
        public string PreviousGradesPath { get; set; }
        public string ParentTaxReturnsPath { get; set; }
        public DateTime DateApplied { get; set; }
    }
}
