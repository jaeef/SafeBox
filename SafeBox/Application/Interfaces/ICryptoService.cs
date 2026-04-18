namespace SafeBox.Application.Interfaces
{
    public interface ICryptoService
    {
        byte[] HashPassword(string password);
        bool VerifyPassword(string password, byte[] hash);
        string HashAdminPassword(string password);
        bool VerifyAdminPassword(string password, string storedHash);
    }
}
