using System;

namespace SafeBox.Domain.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminUsername { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId
        {
            get => AdminId;
            set => AdminId = value;
        }

        public string Username
        {
            get => AdminUsername;
            set => AdminUsername = value;
        }
    }
}
