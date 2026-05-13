using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    public class User
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } 
        public DateTime birthdate { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Course { get; set; }
        public int YearLevel { get; set; }
    }
}
