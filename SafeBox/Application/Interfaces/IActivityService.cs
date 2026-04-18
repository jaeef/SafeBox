using SafeBox.Domain.Entities;
using System.Collections.Generic;

namespace SafeBox.Application.Interfaces
{
    public interface IActivityService
    {
        void LogActivity(int userId, string action, string description);
        IEnumerable<ActivityLog> GetRecentActivities(int userId, int count = 10);
        IEnumerable<ActivityLog> GetAllActivities(int userId);
    }
}
