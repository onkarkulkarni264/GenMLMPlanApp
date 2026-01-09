using GenMLMPlanApp.Models;
using GenMLMPlanApp.ViewModels;

namespace GenMLMPlanApp.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<(bool Success, string Message, User? User)> RegisterAsync(RegisterViewModel model);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByCodeAsync(string userCode);
        Task<bool> IsEmailTakenAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task ToggleUserStatusAsync(int id);
    }
}
