using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using SafeBox.Application.Interfaces;

namespace SafeBox.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService()
        {
            string passphrase = "SafeBoxEncryptionKey2024";
            using (SHA256 sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(passphrase));
            }

            using (MD5 md5 = MD5.Create())
            {
                _iv = md5.ComputeHash(_key);
            }
        }

        public byte[] Encrypt(byte[] data)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(data, 0, data.Length);
                        }
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Encryption failed: {ex.Message}", ex);
            }
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream ms = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (MemoryStream result = new MemoryStream())
                            {
                                cs.CopyTo(result);
                                return result.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Decryption failed: {ex.Message}", ex);
            }
        }
    }
}
