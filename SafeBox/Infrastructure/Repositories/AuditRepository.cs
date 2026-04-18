using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SafeBox.Infrastructure.Repositories
{
    public class AuditRepository
    {
        private readonly string _connectionString;

        public AuditRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void Add(AuditLog auditLog)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO AuditLogs (UserId, Action, Description, Timestamp) 
                                   VALUES (@userId, @action, @description, @timestamp)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", auditLog.UserId);
                        cmd.Parameters.AddWithValue("@action", auditLog.Action);
                        cmd.Parameters.AddWithValue("@description", auditLog.Description ?? string.Empty);
                        cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging audit: {ex.Message}", ex);
            }
        }

        public IEnumerable<AuditLog> GetAll()
        {
            List<AuditLog> logs = new List<AuditLog>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT AuditId, UserId, Action, Description, Timestamp 
                                   FROM AuditLogs 
                                   ORDER BY Timestamp DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new AuditLog
                                {
                                    AuditId = reader.GetInt32(0),
                                    UserId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                    Action = reader.GetString(2),
                                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Timestamp = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving audit logs: {ex.Message}", ex);
            }
            return logs;
        }

        public int GetTotalUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public int GetDeactivatedUserCount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Status = 0";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public int GetPasswordResetCount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM AuditLogs WHERE Action = 'Password Reset'";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        public IEnumerable<AuditLog> GetRecent(int count)
        {
            List<AuditLog> logs = new List<AuditLog>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT TOP (@count) AuditId, UserId, Action, Description, Timestamp 
                                   FROM AuditLogs 
                                   ORDER BY Timestamp DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@count", count);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new AuditLog
                                {
                                    AuditId = reader.GetInt32(0),
                                    UserId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                    Action = reader.GetString(2),
                                    Description = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    Timestamp = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recent audit logs: {ex.Message}", ex);
            }
            return logs;
        }
    }
}
