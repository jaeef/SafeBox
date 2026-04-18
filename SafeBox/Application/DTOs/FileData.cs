using System;

namespace SafeBox.Application.DTOs
{
   
    public class FileData
    {
      
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public string VaultId { get; set; }
        public FileData()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }

        public string GetFormattedSize()
        {
            if (Size < 1024)
                return $"{Size} B";
            else if (Size < 1024 * 1024)
                return $"{Size / 1024.0:F1} KB";
            else if (Size < 1024 * 1024 * 1024)
                return $"{Size / (1024.0 * 1024.0):F1} MB";
            else
                return $"{Size / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }

        public static string GetFileTypeFromExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return "UNKNOWN";

            extension = extension.ToLower().TrimStart('.');

            switch (extension)
            {
                case "pdf":
                    return "PDF FILE";
                case "doc":
                case "docx":
                    return "WORD DOC";
                case "xls":
                case "xlsx":
                    return "EXCEL FILE";
                case "ppt":
                case "pptx":
                    return "POWERPOINT";
                case "txt":
                    return "TEXT FILE";
                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                case "bmp":
                case "webp":
                    return "IMAGE FILE";
                case "mp4":
                case "avi":
                case "mkv":
                case "mov":
                case "wmv":
                    return "VIDEO FILE";
                case "mp3":
                case "wav":
                case "flac":
                case "aac":
                    return "AUDIO FILE";
                case "zip":
                case "rar":
                case "7z":
                case "tar":
                case "gz":
                    return "ARCHIVE";
                case "exe":
                case "msi":
                    return "EXECUTABLE";
                case "html":
                case "htm":
                case "css":
                case "js":
                    return "WEB FILE";
                case "cs":
                case "cpp":
                case "py":
                case "java":
                    return "CODE FILE";
                default:
                    return extension.ToUpper() + " FILE";
            }
        }
    }
}

