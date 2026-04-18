using SafeBox.Application.DTOs;
using SafeBox.Application.Interfaces;
using SafeBox.Application.Interfaces.Repositories;
using SafeBox.Domain.Entities;
using SafeBox.Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace SafeBox.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;

        public AuthService(IUserRepository userRepository, ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
        }


        public UserDto Login(string username, string password)
        {
            ValidateLoginInput(username, password);

            var user = _userRepository.GetByUsername(username);
            
            // Security: Use generic message to prevent username enumeration, 
            // but for business rules (inactive) we can be specific if required by spec.
            if (user == null || !_cryptoService.VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid Username or Password!");
            }

            // Check if user account is active
            if (user.Status.ToLower() == "inactive")
            {
                throw new UnauthorizedException("Your account has been deactivated by an administrator. Please contact your system administrator to reactivate your account.");
            }

            _userRepository.UpdateLastLogin(user.UserId);

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Status = user.Status,
                RoleId = user.RoleId,
                PasswordHash = user.PasswordHash
            };
        }

        public Admin AdminLogin(string username, string password)
        {
            ValidateLoginInput(username, password);

            var admin = _userRepository.GetAdminByUsername(username);
            
            if (admin == null || !_cryptoService.VerifyAdminPassword(password, admin.PasswordHash))
            {
                throw new UnauthorizedException("Invalid Admin Username or Password!");
            }

            if (!admin.Status)
            {
                throw new BusinessRuleException("Your admin account is inactive.");
            }

            return admin;
        }

        public void Register(string username, string email, string password)
        {
            ValidateRegistrationInput(username, email, password);

            if (_userRepository.IsEmailTaken(email, 0))
            {
                throw new BusinessRuleException("Email already exists!");
            }

            if (_userRepository.GetByUsername(username) != null)
            {
                throw new BusinessRuleException("Username already exists!");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = _cryptoService.HashPassword(password),
                RoleId = 2,
                Status = "Inactive",
                CreatedDate = DateTime.Now
            };

            _userRepository.Add(user);
        }

        private void ValidateLoginInput(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ValidationException("Username is required.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password is required.");
        }

        private void ValidateRegistrationInput(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ValidationException("Username is required.");
            if (username.Length < 3)
                throw new ValidationException("Username must be at least 3 characters long.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email is required.");

            // Rule: Correct according to all standard email rules
            // 1. One @ symbol
            // 2. Local part: letters, numbers, dots, underscores, hyphens.
            // 3. Domain part: letters/numbers encoded blocks + dots.
            // 4. Extension: 2-6 letters.
            // 5. No double dots (handled by structure)
            
            string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9]+(\.[a-zA-Z0-9]+)*\.[a-zA-Z]{2,6}$";
            
            if (!Regex.IsMatch(email, pattern))
                throw new ValidationException("Invalid email format. Please follow standard format (e.g. user@example.com).");
            
            // Additional check to prevent double dots if not fully caught by regex structure (though (\.[a-zA-Z0-9]+) helps)
            if (email.Contains(".."))
                throw new ValidationException("Email cannot contain consecutive dots.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password is required.");
            if (password.Length < 6)
                throw new ValidationException("Password must be at least 6 characters long.");
        }
    }
}
