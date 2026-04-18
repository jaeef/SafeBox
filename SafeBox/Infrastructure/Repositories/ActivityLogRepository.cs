using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SafeBox.Infrastructure.Repositories
{
    public class ActivityLogRepository
    {
        private readonly string _connectionString;

        public ActivityLogRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void Add(ActivityLog activityLog)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO ActivityLogs (UserId, Action, Description, Timestamp) 
                                   VALUES (@userId, @action, @description, @timestamp)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", activityLog.UserId);
                        cmd.Parameters.AddWithValue("@action", activityLog.Action);
                        cmd.Parameters.AddWithValue("@description", activityLog.Description ?? string.Empty);
                        cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging activity: {ex.Message}", ex);
            }
        }

        public IEnumerable<ActivityLog> GetRecentByUserId(int userId, int count = 10)
        {
            List<ActivityLog> logs = new List<ActivityLog>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT TOP (@count) ActivityId, UserId, Action, Description, Timestamp 
                                   FROM ActivityLogs 
                                   WHERE UserId = @userId 
                                   ORDER BY Timestamp DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@count", count);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new ActivityLog
                                {
                                    ActivityId = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
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
                throw new Exception($"Error retrieving activity logs: {ex.Message}", ex);
            }
            return logs;
        }

        public IEnumerable<ActivityLog> GetAllByUserId(int userId)
        {
            List<ActivityLog> logs = new List<ActivityLog>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT ActivityId, UserId, Action, Description, Timestamp 
                                   FROM ActivityLogs 
                                   WHERE UserId = @userId 
                                   ORDER BY Timestamp DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                logs.Add(new ActivityLog
                                {
                                    ActivityId = reader.GetInt32(0),
                                    UserId = reader.GetInt32(1),
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
                throw new Exception($"Error retrieving activity logs: {ex.Message}", ex);
            }
            return logs;
        }
    }
}
