using System;

namespace SafeBox.Domain.Entities
{
    /// <summary>
    /// Represents a user in the SafeBox system.
    /// Maps to Users table.
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        // PasswordHash is stored as byte[] in the database (VARBINARY)
        public byte[] PasswordHash { get; set; }

        // Status is stored as string "Active" or "Inactive" for compatibility
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Additional properties used by existing code
        public int RoleId { get; set; }
        public DateTime? LastLoginDate { get; set; }

        // Alias for backwards compatibility
        public DateTime CreatedDate
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }
    }
}
