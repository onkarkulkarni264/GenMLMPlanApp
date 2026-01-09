using GenMLMPlanApp.ViewModels;

namespace GenMLMPlanApp.Services
{
    public interface IIncomeService
    {
        Task<DashboardViewModel> CalculateDashboardStatsAsync(int userId);
    }
}
