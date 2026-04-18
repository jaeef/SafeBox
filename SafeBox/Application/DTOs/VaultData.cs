using System;
using System.Collections.Generic;

namespace SafeBox.Application.DTOs
{

    public class VaultData
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<FileData> Files { get; set; }
        public long TotalSize { get; set; }
        public int FileCount { get; set; }
        public VaultData()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            Files = new List<FileData>();
            TotalSize = 0;
            FileCount = 0;
        }


        public string GetFormattedSize()
        {
            if (TotalSize < 1024)
                return $"{TotalSize} B";
            else if (TotalSize < 1024 * 1024)
                return $"{TotalSize / 1024.0:F1} KB";
            else if (TotalSize < 1024 * 1024 * 1024)
                return $"{TotalSize / (1024.0 * 1024.0):F1} MB";
            else
                return $"{TotalSize / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }
    }
}

