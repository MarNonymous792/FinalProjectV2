using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectV2
{
    public static class Session
    {
        public static User CurrentUser { get; set; }

        public static event Action OnDatabaseChanged;
        public static void TriggerDatabaseChanged()
        {
            OnDatabaseChanged?.Invoke();
        }
    }
}
