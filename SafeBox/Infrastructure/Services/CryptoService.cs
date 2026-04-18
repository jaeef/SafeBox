using System;
using System.Security.Cryptography;
using System.Text;
using SafeBox.Application.Interfaces;

namespace SafeBox.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        public byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password, byte[] hash)
        {
            byte[] newHash = HashPassword(password);
            if (newHash == null || hash == null || newHash.Length != hash.Length)
                return false;

            for (int i = 0; i < newHash.Length; i++)
            {
                if (newHash[i] != hash[i])
                    return false;
            }
            return true;
        }

        public string HashAdminPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyAdminPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            string computedHash = HashAdminPassword(password);
            return string.Equals(computedHash, storedHash, StringComparison.Ordinal);
        }
    }
}
