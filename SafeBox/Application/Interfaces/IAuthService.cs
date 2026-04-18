using SafeBox.Application.DTOs;
using SafeBox.Domain.Entities;

namespace SafeBox.Application.Interfaces
{
    public interface IAuthService
    {
        UserDto Login(string username, string password);
        Admin AdminLogin(string username, string password);
        void Register(string username, string email, string password);
    }
}
