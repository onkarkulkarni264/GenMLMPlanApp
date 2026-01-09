using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenMLMPlanApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty; // e.g. REG1001

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone]
        public string Mobile { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        // Foreign Key for Sponsor (Self-Referencing)
        public int? SponsorId { get; set; }

        // Navigation Property: The user who referred this user
        [ForeignKey("SponsorId")]
        public virtual User? Sponsor { get; set; }

        // Navigation Property: The users this user has referred directly
        [InverseProperty("Sponsor")]
        public virtual ICollection<User> DirectReferrals { get; set; } = new List<User>();

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
