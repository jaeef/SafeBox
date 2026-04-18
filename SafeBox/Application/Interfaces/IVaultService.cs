using SafeBox.Domain.Entities;
using System.Collections.Generic;

namespace SafeBox.Application.Interfaces
{
    public interface IVaultService
    {
        Vault CreateVault(string vaultName, string description, int userId);
        Vault GetVaultById(int vaultId);
        IEnumerable<Vault> GetVaultsByUserId(int userId);
        void DeleteVault(int vaultId, int userId);
        int GetFileCount(int vaultId);
        long GetTotalSize(int vaultId);
        int GetVaultCount(int userId);
        IEnumerable<string> GetVaultNames(int userId);
    }
}
