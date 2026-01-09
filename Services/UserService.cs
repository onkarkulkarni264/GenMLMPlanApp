using GenMLMPlanApp.Data;
using GenMLMPlanApp.Models;
using GenMLMPlanApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GenMLMPlanApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash)) return null;

            return user;
        }

        public async Task<(bool Success, string Message, User? User)> RegisterAsync(RegisterViewModel model)
        {
            if (await IsEmailTakenAsync(model.Email))
                return (false, "Email already exists.", null);

            User? sponsor = null;
            if (!string.IsNullOrEmpty(model.SponsorId))
            {
                sponsor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.SponsorId);
                if (sponsor == null)
                    return (false, "Invalid Sponsor ID.", null);
            }

            // Generate User ID
            int nextId = await _context.Users.CountAsync() + 1001;
            string newUserId = $"REG{nextId}";
            
            // Double check uniqueness (unlikely collision but good practice)
            while (await _context.Users.AnyAsync(u => u.UserId == newUserId))
            {
                nextId++;
                newUserId = $"REG{nextId}";
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Mobile = model.Mobile,
                UserId = newUserId,
                SponsorId = sponsor?.Id,
                PasswordHash = HashPassword(model.Password),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return (true, "Registration successful.", user);
            }
            catch (Exception ex)
            {
                // In a real app, log error
                return (false, $"Error creating user: {ex.Message}", null);
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Sponsor).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Sponsor)
                .Include(u => u.DirectReferrals)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByCodeAsync(string userCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userCode);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Sponsor).OrderByDescending(u => u.CreatedAt).ToListAsync();
        }

        public async Task ToggleUserStatusAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _context.SaveChangesAsync();
            }
        }

        // Simple hashing for demo purposes. In production, use BCrypt or Argon2.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
