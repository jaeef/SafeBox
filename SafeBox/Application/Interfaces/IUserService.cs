using SafeBox.Domain.Entities;
using System.Collections.Generic;

namespace SafeBox.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> SearchUsers(string searchTerm);
        int GetTotalUserCount();
        int GetActiveUserCount();
        int GetInactiveUserCount();
        bool IsUserActive(int userId);
        void ActivateUser(int userId);
        void DeactivateUser(int userId);
        User GetUserById(int id);
        void UpdateUser(User user);
        bool IsEmailTaken(string email, int excludeUserId);
        IEnumerable<User> GetRecentUsers(int count);
        void UpdatePassword(int userId, byte[] passwordHash);
        byte[] GetPasswordHash(int userId);
    }
}
