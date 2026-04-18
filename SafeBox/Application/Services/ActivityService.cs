using SafeBox.Application.Interfaces;
using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Repositories;
using System;
using System.Collections.Generic;

namespace SafeBox.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ActivityLogRepository _activityLogRepository;

        public ActivityService()
        {
            _activityLogRepository = new ActivityLogRepository();
        }

        public ActivityService(ActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        public void LogActivity(int userId, string action, string description)
        {
            try
            {
                _activityLogRepository.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = action,
                    Description = description,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to log activity: {ex.Message}");
            }
        }

        public IEnumerable<ActivityLog> GetRecentActivities(int userId, int count = 10)
        {
            try
            {
                return _activityLogRepository.GetRecentByUserId(userId, count);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve activities: {ex.Message}", ex);
            }
        }

        public IEnumerable<ActivityLog> GetAllActivities(int userId)
        {
            try
            {
                return _activityLogRepository.GetAllByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve activities: {ex.Message}", ex);
            }
        }
    }
}
