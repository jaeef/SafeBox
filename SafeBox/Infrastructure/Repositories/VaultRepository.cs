using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SafeBox.Infrastructure.Repositories
{
    public class VaultRepository
    {
        private readonly string _connectionString;

        public VaultRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void Add(Vault vault)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO Vaults (VaultName, UserId, CreatedDate, Description) 
                                   VALUES (@name, @userId, @createdDate, @description);
                                   SELECT CAST(SCOPE_IDENTITY() as int);";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@name", vault.VaultName);
                        cmd.Parameters.AddWithValue("@userId", vault.UserId);
                        cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@description", vault.Description ?? string.Empty);

                        vault.VaultId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating vault: {ex.Message}", ex);
            }
        }

        public Vault GetById(int vaultId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT VaultId, VaultName, UserId, CreatedDate, Description FROM Vaults WHERE VaultId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", vaultId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Vault
                                {
                                    VaultId = reader.GetInt32(0),
                                    VaultName = reader.GetString(1),
                                    UserId = reader.GetInt32(2),
                                    CreatedDate = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                    Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vault: {ex.Message}", ex);
            }
            return null;
        }

        public IEnumerable<Vault> GetByUserId(int userId)
        {
            List<Vault> vaults = new List<Vault>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT VaultId, VaultName, UserId, CreatedDate, Description 
                                   FROM Vaults 
                                   WHERE UserId = @userId 
                                   ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vaults.Add(new Vault
                                {
                                    VaultId = reader.GetInt32(0),
                                    VaultName = reader.GetString(1),
                                    UserId = reader.GetInt32(2),
                                    CreatedDate = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                    Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving vaults: {ex.Message}", ex);
            }
            return vaults;
        }

        public void Delete(int vaultId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Vaults WHERE VaultId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", vaultId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting vault: {ex.Message}", ex);
            }
        }

        public int GetFileCount(int vaultId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Files WHERE VaultId = @vaultId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@vaultId", vaultId);
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public long GetTotalSize(int vaultId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT ISNULL(SUM(FileSize), 0) FROM Files WHERE VaultId = @vaultId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@vaultId", vaultId);
                        object result = cmd.ExecuteScalar();
                        return result != DBNull.Value ? Convert.ToInt64(result) : 0;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public int GetVaultCount(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Vaults WHERE UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public IEnumerable<string> GetVaultNames(int userId)
        {
            List<string> names = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT VaultName FROM Vaults WHERE UserId = @userId ORDER BY VaultName";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                names.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return names;
        }
    }
}
