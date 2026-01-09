using GenMLMPlanApp.Data;
using GenMLMPlanApp.Models;
using GenMLMPlanApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GenMLMPlanApp.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly AppDbContext _context;

        // constants
        private const decimal INCOME_LEVEL_1 = 100m;
        private const decimal INCOME_LEVEL_2 = 50m;
        private const decimal INCOME_LEVEL_3 = 25m;

        public IncomeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> CalculateDashboardStatsAsync(int userId)
        {
            // Load user with 3 levels of descendants hierarchy
            var user = await _context.Users
                .Include(u => u.DirectReferrals)
                    .ThenInclude(l2 => l2.DirectReferrals)
                        .ThenInclude(l3 => l3.DirectReferrals)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return new DashboardViewModel();

            var model = new DashboardViewModel
            {
                CurrentUser = user,
                DirectReferrals = user.DirectReferrals.ToList(),
                DirectReferralsCount = user.DirectReferrals.Count()
            };

            // Calculate Income & Team Size
            // Level 1
            int countL1 = user.DirectReferrals.Count();
            decimal incomeL1 = countL1 * INCOME_LEVEL_1;

            // Level 2
            var level2Users = user.DirectReferrals.SelectMany(r => r.DirectReferrals).ToList();
            int countL2 = level2Users.Count();
            decimal incomeL2 = countL2 * INCOME_LEVEL_2;

            // Level 3
            var level3Users = level2Users.SelectMany(r => r.DirectReferrals).ToList();
            int countL3 = level3Users.Count();
            decimal incomeL3 = countL3 * INCOME_LEVEL_3;

            model.TeamSize = countL1 + countL2 + countL3;
            model.TotalIncome = incomeL1 + incomeL2 + incomeL3;

            return model;
        }
    }
}
