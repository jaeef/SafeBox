using SafeBox.Application.Interfaces;
using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Repositories;
using SafeBox.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace SafeBox.Application.Services
{
    public class VaultService : IVaultService
    {
        private readonly VaultRepository _vaultRepository;
        private readonly ActivityLogRepository _activityLogRepository;
        private readonly IUserService _userService;

        public VaultService()
        {
            _vaultRepository = new VaultRepository();
            _activityLogRepository = new ActivityLogRepository();
            _userService = new UserService();
        }

        public VaultService(VaultRepository vaultRepository, ActivityLogRepository activityLogRepository, IUserService userService)
        {
            _vaultRepository = vaultRepository;
            _activityLogRepository = activityLogRepository;
            _userService = userService;
        }

        public Vault CreateVault(string vaultName, string description, int userId)
        {
            EnsureUserActive(userId);

            if (string.IsNullOrWhiteSpace(vaultName))
            {
                throw new ValidationException("Vault name is required.");
            }

            if (vaultName.Length > 50)
            {
                throw new ValidationException("Vault name cannot exceed 50 characters.");
            }

            try
            {
                // potentially check for duplicates here if requirement exists, e.g.
                // if (_vaultRepository.Exists(vaultName, userId)) throw new BusinessRuleException("Vault name already exists.");

                var vault = new Vault
                {
                    VaultName = vaultName.Trim(),
                    Description = description?.Trim(),
                    UserId = userId,
                    CreatedDate = DateTime.Now
                };

                _vaultRepository.Add(vault);

                _activityLogRepository.Add(new ActivityLog
                {
                    UserId = userId,
                    Action = vault.VaultName,
                    Description = "New Vault Created",
                    Timestamp = DateTime.Now
                });

                return vault;
            }
            catch (Exception ex)
            {
                // Log technical error (if logger existed)
                // Rethrow reasonable exception
                throw new Exception($"Failed to create vault: {ex.Message}", ex);
            }
        }

        public Vault GetVaultById(int vaultId)
        {
            try
            {
                return _vaultRepository.GetById(vaultId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve vault: {ex.Message}", ex);
            }
        }

        public IEnumerable<Vault> GetVaultsByUserId(int userId)
        {
            try
            {
                return _vaultRepository.GetByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve vaults: {ex.Message}", ex);
            }
        }

        public void DeleteVault(int vaultId, int userId)
        {
            EnsureUserActive(userId); // Strict Access Control

            try
            {
                var vault = _vaultRepository.GetById(vaultId);
                if (vault != null)
                {
                    string vaultName = vault.VaultName;
                    _vaultRepository.Delete(vaultId);

                    _activityLogRepository.Add(new ActivityLog
                    {
                        UserId = userId,
                        Action = vaultName,
                        Description = "Vault Deleted",
                        Timestamp = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete vault: {ex.Message}", ex);
            }
        }

        public int GetFileCount(int vaultId)
        {
            return _vaultRepository.GetFileCount(vaultId);
        }

        public long GetTotalSize(int vaultId)
        {
            return _vaultRepository.GetTotalSize(vaultId);
        }

        public int GetVaultCount(int userId)
        {
            return _vaultRepository.GetVaultCount(userId);
        }

        public IEnumerable<string> GetVaultNames(int userId)
        {
            return _vaultRepository.GetVaultNames(userId);
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
