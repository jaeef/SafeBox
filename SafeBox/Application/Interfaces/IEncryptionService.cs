namespace SafeBox.Application.Interfaces
{
    public interface IEncryptionService
    {
        byte[] Encrypt(byte[] data);
        byte[] Decrypt(byte[] encryptedData);
    }
}
