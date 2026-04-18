using SafeBox.Application.Interfaces;
using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Repositories;
using SafeBox.Infrastructure.Services;
using SafeBox.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FileEntity = SafeBox.Domain.Entities.File;

namespace SafeBox.Application.Services
{
    public class FileService : IFileService
    {
        private readonly FileRepository _fileRepository;
        private readonly ActivityLogRepository _activityLogRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly VaultRepository _vaultRepository;
        private readonly IUserService _userService;

        public FileService()
        {
            _fileRepository = new FileRepository();
            _activityLogRepository = new ActivityLogRepository();
            _encryptionService = new EncryptionService(); 
            _vaultRepository = new VaultRepository();
            _userService = new UserService();
        }

        public FileService(FileRepository fileRepository, ActivityLogRepository activityLogRepository, IEncryptionService encryptionService, VaultRepository vaultRepository, IUserService userService)
        {
            _fileRepository = fileRepository;
            _activityLogRepository = activityLogRepository;
            _encryptionService = encryptionService;
            _vaultRepository = vaultRepository;
            _userService = userService;
        }

        public FileEntity UploadFile(string filePath, int vaultId, int userId)
        {
            EnsureUserActive(userId);
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new ValidationException("File path cannot be empty.");

                if (!System.IO.File.Exists(filePath))
                    throw new NotFoundException($"Source file not found: {filePath}");

                var fileInfo = new System.IO.FileInfo(filePath);
                
                // Max file size validation (e.g., 50MB)
                long maxSizeBytes = 50 * 1024 * 1024; 
                if (fileInfo.Length > maxSizeBytes)
                {
                    throw new ValidationException($"File is too large. Maximum size is 50MB.");
                }

                // Empty file validation
                if (fileInfo.Length == 0)
                {
                    throw new ValidationException("Cannot upload empty files.");
                }

                string fileName = fileInfo.Name;
                string extension = Path.GetExtension(fileName);

                // Block restricted extensions
                string[] separateExtensions = { ".exe", ".bat", ".cmd", ".sh", ".dll" };
                if (separateExtensions.Contains(extension.ToLower()))
                {
                    throw new ValidationException("Executable files are not allowed for security reasons.");
                }

                string fileType = GetFileTypeFromExtension(extension);
                byte[] fileData = System.IO.File.ReadAllBytes(filePath);
                byte[] encryptedData = _encryptionService.Encrypt(fileData);

                var file = new FileEntity
                {
                    FileName = fileName,
                    FileType = fileType,
                    FileSize = fileInfo.Length,
                    EncryptedData = encryptedData,
                    VaultId = vaultId,
                    UploadDate = DateTime.Now
                };

                _fileRepository.Add(file);

                var vault = _vaultRepository.GetById(vaultId);
                string vaultName = vault?.VaultName ?? "Unknown Vault";

                _activityLogRepository.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = fileName,
                    Description = vaultName,
                    Timestamp = DateTime.Now
                });

                return file;
            }
            catch (ValidationException) { throw; } // Bubbling up business exceptions
            catch (NotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload file: {ex.Message}", ex);
            }
        }

        public FileEntity GetFileById(int fileId)
        {
            try
            {
                return _fileRepository.GetById(fileId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve file: {ex.Message}", ex);
            }
        }

        public IEnumerable<FileEntity> GetFilesByVaultId(int vaultId)
        {
            try
            {
                return _fileRepository.GetByVaultId(vaultId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve files: {ex.Message}", ex);
            }
        }

        public IEnumerable<FileEntity> GetAllFiles(int userId)
        {
            try
            {
                return _fileRepository.GetAllFiles(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve all files: {ex.Message}", ex);
            }
        }

        public void DeleteFile(int fileId, int userId)
        {
            EnsureUserActive(userId);
            try
            {
                var file = _fileRepository.GetById(fileId);
                if (file != null)
                {
                    var vault = _vaultRepository.GetById(file.VaultId);
                    string vaultName = vault?.VaultName ?? "Unknown Vault";

                    _fileRepository.Delete(fileId);

                    _activityLogRepository.Add(new ActivityLog
                    {
                        UserId = userId,
                        Action = file.FileName,
                        Description = vaultName,
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete file: {ex.Message}", ex);
            }
        }

        public void DownloadEncryptedFile(int fileId, string destinationPath, int userId)
        {
            EnsureUserActive(userId);
            try
            {
                var file = _fileRepository.GetById(fileId);
                if (file == null)
                    throw new NotFoundException("File not found.");

                if (file.EncryptedData == null || file.EncryptedData.Length == 0)
                    throw new Exception("No encrypted data found for this file.");

                // Convert encrypted bytes to Base64 string
                string base64Content = Convert.ToBase64String(file.EncryptedData);

                // Write Base64 string to file as text
                System.IO.File.WriteAllText(destinationPath, base64Content);

                var vault = _vaultRepository.GetById(file.VaultId);
                string vaultName = vault?.VaultName ?? "Unknown Vault";

                _activityLogRepository.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = "Encrypted Download",
                    Description = $"Downloaded encrypted file: {file.FileName} from {vaultName}",
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download encrypted file: {ex.Message}", ex);
            }
        }

        public void DownloadFile(int fileId, string destinationPath, int userId)
        {
            EnsureUserActive(userId);
            try
            {
                var file = _fileRepository.GetById(fileId) ?? throw new Exception("File not found");

                byte[] decryptedData = _encryptionService.Decrypt(file.EncryptedData);
                System.IO.File.WriteAllBytes(destinationPath, decryptedData);

                var vault = _vaultRepository.GetById(file.VaultId);
                string vaultName = vault?.VaultName ?? "Unknown Vault";

                _activityLogRepository.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = file.FileName,
                    Description = vaultName,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download file: {ex.Message}", ex);
            }
        }

        public long GetTotalStorageSize(int userId)
        {
            return _fileRepository.GetTotalStorageSize(userId);
        }

        private string GetFileTypeFromExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return "UNKNOWN";

            extension = extension.ToLower().TrimStart('.');

            switch (extension)
            {
                case "pdf": return "PDF FILE";
                case "doc":
                case "docx": return "WORD DOC";
                case "xls":
                case "xlsx": return "EXCEL FILE";
                case "ppt":
                case "pptx": return "POWERPOINT";
                case "txt": return "TEXT FILE";
                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                case "bmp":
                case "webp": return "IMAGE FILE";
                case "mp4":
                case "avi":
                case "mkv":
                case "mov":
                case "wmv": return "VIDEO FILE";
                case "mp3":
                case "wav":
                case "flac":
                case "aac": return "AUDIO FILE";
                case "zip":
                case "rar":
                case "7z":
                case "tar":
                case "gz": return "ARCHIVE";
                default: return extension.ToUpper() + " FILE";
            }
        }

        public int GetTotalFileCount(int userId)
        {
            return _fileRepository.GetTotalFileCount(userId);
        }

        public IEnumerable<string> GetFileNames(int userId)
        {
            return _fileRepository.GetFileNames(userId);
        }

        public IEnumerable<FileEntity> SearchFiles(string searchTerm, int userId)
        {
            return _fileRepository.SearchFiles(searchTerm, userId);
        }

        private void EnsureUserActive(int userId)
        {
            if (!_userService.IsUserActive(userId))
            {
                throw new UnauthorizedException("Your account is inactive. Please contact administrator.");
            }
        }
    }
}
