using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SafeBox.Infrastructure.Repositories
{
    public class FileRepository
    {
        private readonly string _connectionString;

        public FileRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void Add(File file)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO Files (FileName, FileType, FileSize, EncryptedData, VaultId, UploadDate) 
                                   VALUES (@fileName, @fileType, @fileSize, @encryptedData, @vaultId, @uploadDate);
                                   SELECT CAST(SCOPE_IDENTITY() as int);";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fileName", file.FileName);
                        cmd.Parameters.AddWithValue("@fileType", file.FileType ?? string.Empty);
                        cmd.Parameters.AddWithValue("@fileSize", file.FileSize);
                        cmd.Parameters.AddWithValue("@encryptedData", file.EncryptedData ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@vaultId", file.VaultId);
                        cmd.Parameters.AddWithValue("@uploadDate", DateTime.Now);

                        file.FileId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }

        public File GetById(int fileId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT FileId, FileName, FileType, FileSize, EncryptedData, VaultId, UploadDate FROM Files WHERE FileId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", fileId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new File
                                {
                                    FileId = reader.GetInt32(0),
                                    FileName = reader.GetString(1),
                                    FileType = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    FileSize = reader.GetInt64(3),
                                    EncryptedData = reader.IsDBNull(4) ? null : (byte[])reader[4],
                                    VaultId = reader.GetInt32(5),
                                    UploadDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving file: {ex.Message}", ex);
            }
            return null;
        }

        public IEnumerable<File> GetByVaultId(int vaultId)
        {
            List<File> files = new List<File>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT FileId, FileName, FileType, FileSize, EncryptedData, VaultId, UploadDate 
                                   FROM Files 
                                   WHERE VaultId = @vaultId 
                                   ORDER BY UploadDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@vaultId", vaultId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                files.Add(new File
                                {
                                    FileId = reader.GetInt32(0),
                                    FileName = reader.GetString(1),
                                    FileType = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    FileSize = reader.GetInt64(3),
                                    EncryptedData = reader.IsDBNull(4) ? null : (byte[])reader[4],
                                    VaultId = reader.GetInt32(5),
                                    UploadDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving files: {ex.Message}", ex);
            }
            return files;
        }

        public IEnumerable<File> GetAllFiles(int userId)
        {
            List<File> files = new List<File>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT f.FileId, f.FileName, f.FileType, f.FileSize, f.EncryptedData, f.VaultId, f.UploadDate
                                   FROM Files f
                                   INNER JOIN Vaults v ON f.VaultId = v.VaultId
                                   WHERE v.UserId = @userId
                                   ORDER BY f.UploadDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                files.Add(new File
                                {
                                    FileId = reader.GetInt32(0),
                                    FileName = reader.GetString(1),
                                    FileType = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    FileSize = reader.GetInt64(3),
                                    EncryptedData = reader.IsDBNull(4) ? null : (byte[])reader[4],
                                    VaultId = reader.GetInt32(5),
                                    UploadDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all files: {ex.Message}", ex);
            }
            return files;
        }

        public void Delete(int fileId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Files WHERE FileId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", fileId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}", ex);
            }
        }

        public long GetTotalStorageSize(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT ISNULL(SUM(f.FileSize), 0)
                                   FROM Files f
                                   INNER JOIN Vaults v ON f.VaultId = v.VaultId
                                   WHERE v.UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
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

        public int GetTotalFileCount(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT COUNT(f.FileId)
                                   FROM Files f
                                   INNER JOIN Vaults v ON f.VaultId = v.VaultId
                                   WHERE v.UserId = @userId";

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

        public IEnumerable<string> GetFileNames(int userId)
        {
            List<string> names = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT f.FileName
                                   FROM Files f
                                   INNER JOIN Vaults v ON f.VaultId = v.VaultId
                                   WHERE v.UserId = @userId
                                   ORDER BY f.FileName";

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

        public IEnumerable<File> SearchFiles(string searchTerm, int userId)
        {
            List<File> files = new List<File>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT f.FileId, f.FileName, f.FileType, f.FileSize, f.EncryptedData, f.VaultId, f.UploadDate
                                   FROM Files f
                                   INNER JOIN Vaults v ON f.VaultId = v.VaultId
                                   WHERE v.UserId = @userId AND f.FileName COLLATE SQL_Latin1_General_CP1_CI_AI LIKE @searchTerm
                                   ORDER BY f.FileName";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                files.Add(new File
                                {
                                    FileId = reader.GetInt32(0),
                                    FileName = reader.GetString(1),
                                    FileType = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    FileSize = reader.GetInt64(3),
                                    EncryptedData = reader.IsDBNull(4) ? null : (byte[])reader[4],
                                    VaultId = reader.GetInt32(5),
                                    UploadDate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching files: {ex.Message}", ex);
            }
            return files;
        }
    }
}
