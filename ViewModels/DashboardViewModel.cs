using System.Collections.Generic;
using GenMLMPlanApp.Models;

namespace GenMLMPlanApp.ViewModels
{
    public class DashboardViewModel
    {
        public User CurrentUser { get; set; } = new User();
        
        public int DirectReferralsCount { get; set; } // Level 1
        public int TeamSize { get; set; } // Total L1 + L2 + L3
        public decimal TotalIncome { get; set; }

        public List<User> DirectReferrals { get; set; } = new List<User>();
    }
}
