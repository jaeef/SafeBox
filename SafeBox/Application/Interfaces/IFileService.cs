using SafeBox.Domain.Entities;
using System.Collections.Generic;
using FileEntity = SafeBox.Domain.Entities.File;

namespace SafeBox.Application.Interfaces
{
    public interface IFileService
    {
        FileEntity UploadFile(string filePath, int vaultId, int userId);
        FileEntity GetFileById(int fileId);
        IEnumerable<FileEntity> GetFilesByVaultId(int vaultId);
        IEnumerable<FileEntity> GetAllFiles(int userId);
        void DeleteFile(int fileId, int userId);
        void DownloadFile(int fileId, string destinationPath, int userId);
        void DownloadEncryptedFile(int fileId, string destinationPath, int userId);
        long GetTotalStorageSize(int userId);
        int GetTotalFileCount(int userId);
        IEnumerable<string> GetFileNames(int userId);
        IEnumerable<FileEntity> SearchFiles(string searchTerm, int userId);
    }
}
