using System;

namespace SafeBox.Domain.Entities
{
    /// <summary>
    /// Represents a vault (secure file container) in the SafeBox system.
    /// Maps to Vault table (vault_id IDENTITY(300,9)).
    /// </summary>
    public class Vault
    {
        public int VaultId { get; set; }
        public string VaultName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Alias for backwards compatibility with existing code
        public DateTime CreatedDate
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }
    }
}
