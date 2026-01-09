using GenMLMPlanApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenMLMPlanApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<IActionResult> Tree()
        {
            // Get root users (users with no sponsor or sponsor is null)
            // But usually in MLM there is one root. 
            // We'll get all users and build the tree in the view or here.
            
            // Allow querying for specific root if needed, otherwise start from system root.
            var users = await _userService.GetAllUsersAsync();
            
            // Filter to find top-level roots (SponsorId is null)
            var roots = users.Where(u => u.SponsorId == null).ToList();
            
            // To make sure we have the full structure in memory (EF might need Include, but GetAllUsersAsync has Includes)
            return View(roots);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _userService.ToggleUserStatusAsync(id);
            return RedirectToAction("Index");
        }
    }
}
