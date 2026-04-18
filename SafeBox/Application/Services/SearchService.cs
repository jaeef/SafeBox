using SafeBox.Domain.Entities;
using SafeBox.Application.Services;
using SafeBox.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SafeBox.Application.Services
{
    public class SearchService
    {
        private readonly FileService _fileService;
        private readonly VaultRepository _vaultRepository;

        public SearchService()
        {
            _fileService = new FileService();
            _vaultRepository = new VaultRepository();
        }

        public IEnumerable<File> SearchFiles(string searchTerm, int userId)
        {
            try
            {
                return _fileService.SearchFiles(searchTerm, userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Search failed: {ex.Message}", ex);
            }
        }

        public IEnumerable<Vault> SearchVaults(string searchTerm, int userId)
        {
            try
            {
                var allVaults = _vaultRepository.GetByUserId(userId);
                return allVaults.Where(v =>
                    v.VaultName.ToLower().Contains(searchTerm.ToLower()) ||
                    (v.Description != null && v.Description.ToLower().Contains(searchTerm.ToLower())))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Search failed: {ex.Message}", ex);
            }
        }

        public class SearchResult
        {
            public int FileId { get; set; }
            public string FileName { get; set; }
            public int VaultId { get; set; }
            public string VaultName { get; set; }
            public string Type { get; set; }
        }

        public IEnumerable<SearchResult> SearchAll(string searchTerm, int userId)
        {
            List<SearchResult> results = new List<SearchResult>();

            try
            {
                var files = SearchFiles(searchTerm, userId);
                foreach (var file in files)
                {
                    var vault = _vaultRepository.GetById(file.VaultId);
                    results.Add(new SearchResult
                    {
                        FileId = file.FileId,
                        FileName = file.FileName,
                        VaultId = file.VaultId,
                        VaultName = vault?.VaultName ?? "Unknown",
                        Type = "File"
                    });
                }

                var vaults = SearchVaults(searchTerm, userId);
                foreach (var vault in vaults)
                {
                    results.Add(new SearchResult
                    {
                        FileId = 0,
                        FileName = vault.VaultName,
                        VaultId = vault.VaultId,
                        VaultName = vault.VaultName,
                        Type = "Vault"
                    });
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception($"Search failed: {ex.Message}", ex);
            }
        }
    }
}
