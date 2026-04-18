using System;

namespace SafeBox.Domain.Entities
{
    /// <summary>
    /// Represents user activity log entry.
    /// Maps to ActivityLog table (activity_id IDENTITY(600,15)).
    /// </summary>
    public class ActivityLog
    {
        public int ActivityId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }

        // Alias for backwards compatibility with existing code
        public string Description
        {
            get => Details;
            set => Details = value;
        }
    }
}
