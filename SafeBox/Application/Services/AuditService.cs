using SafeBox.Infrastructure.Repositories;
using SafeBox.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SafeBox.Application.Services
{
    public class AuditService
    {
        private readonly AuditRepository _auditRepository;

        public AuditService()
        {
            _auditRepository = new AuditRepository();
        }

        public void LogAction(int userId, string action, string description)
        {
            try
            {
                _auditRepository.Add(new AuditLog
                {
                    UserId = userId,
                    Action = action,
                    Description = description,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to log audit: {ex.Message}");
            }
        }

        public IEnumerable<AuditLog> GetAllAuditLogs()
        {
            try
            {
                return _auditRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve audit logs: {ex.Message}", ex);
            }
        }

        public int GetTotalUsers()
        {
            return _auditRepository.GetTotalUsers();
        }

        public int GetDeactivatedUserCount()
        {
            return _auditRepository.GetDeactivatedUserCount();
        }

        public int GetPasswordResetCount()
        {
            return _auditRepository.GetPasswordResetCount();
        }

        public IEnumerable<AuditLog> GetRecentAuditLogs(int count)
        {
            return _auditRepository.GetRecent(count);
        }
    }
}
