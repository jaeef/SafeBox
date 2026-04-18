using SafeBox.Domain.Entities;
using System.Collections.Generic;

namespace SafeBox.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void UpdateLastLogin(int userId);
        bool UserExists(string username, string email);
        IEnumerable<User> GetAll();
        IEnumerable<User> Search(string searchTerm);
        int GetTotalUserCount();
        int GetActiveUserCount();
        int GetInactiveUserCount();
        void ActivateUser(int userId);
        void DeactivateUser(int userId);
        bool IsEmailTaken(string email, int excludeUserId);
        IEnumerable<User> GetRecent(int count);
        void UpdatePassword(int userId, byte[] passwordHash);
        byte[] GetPasswordHash(int userId);
        Admin GetAdminByUsername(string username);
    }
}

