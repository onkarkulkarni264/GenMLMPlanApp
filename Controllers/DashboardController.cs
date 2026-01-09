using GenMLMPlanApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GenMLMPlanApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IIncomeService _incomeService;
        private readonly IUserService _userService;

        public DashboardController(IIncomeService incomeService, IUserService userService)
        {
            _incomeService = incomeService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdStr, out int userId))
            {
                var viewModel = await _incomeService.CalculateDashboardStatsAsync(userId);
                return View(viewModel);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
