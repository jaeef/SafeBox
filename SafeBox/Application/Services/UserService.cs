using SafeBox.Application.Interfaces;
using SafeBox.Application.Interfaces.Repositories;
using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Repositories;
using System.Collections.Generic;

namespace SafeBox.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<User> SearchUsers(string searchTerm)
        {
            return _userRepository.Search(searchTerm);
        }

        public int GetTotalUserCount()
        {
            return _userRepository.GetTotalUserCount();
        }

        public int GetActiveUserCount()
        {
            return _userRepository.GetActiveUserCount();
        }

        public int GetInactiveUserCount()
        {
            return _userRepository.GetInactiveUserCount();
        }

        public bool IsUserActive(int userId)
        {
            var user = _userRepository.GetById(userId);
            return user != null && user.Status == "Active";
        }

        public void ActivateUser(int userId)
        {
            _userRepository.ActivateUser(userId);
        }

        public void DeactivateUser(int userId)
        {
            _userRepository.DeactivateUser(userId);
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public bool IsEmailTaken(string email, int excludeUserId)
        {
            return _userRepository.IsEmailTaken(email, excludeUserId);
        }

        public IEnumerable<User> GetRecentUsers(int count)
        {
            return _userRepository.GetRecent(count);
        }

        public void UpdatePassword(int userId, byte[] passwordHash)
        {
            _userRepository.UpdatePassword(userId, passwordHash);
        }

        public byte[] GetPasswordHash(int userId)
        {
            return _userRepository.GetPasswordHash(userId);
        }
    }
}
