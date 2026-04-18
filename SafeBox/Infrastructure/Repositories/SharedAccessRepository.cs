using SafeBox.Domain.Entities;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SafeBox.Infrastructure.Repositories
{
    public class SharedAccessRepository
    {
        private readonly string _connectionString;

        public SharedAccessRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void Add(SharedAcess sharedAccess)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO SharedAccess (FileId, SharedByUserId, SharedToUserId, SharedDate) 
                                   VALUES (@fileId, @sharedByUserId, @sharedToUserId, @sharedDate)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fileId", sharedAccess.FileId);
                        cmd.Parameters.AddWithValue("@sharedByUserId", sharedAccess.SharedByUserId);
                        cmd.Parameters.AddWithValue("@sharedToUserId", sharedAccess.SharedToUserId);
                        cmd.Parameters.AddWithValue("@sharedDate", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sharing file: {ex.Message}", ex);
            }
        }

        public IEnumerable<SharedAcess> GetByFileId(int fileId)
        {
            List<SharedAcess> sharedAccesses = new List<SharedAcess>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT ShareId, FileId, SharedByUserId, SharedToUserId, SharedDate 
                                   FROM SharedAccess 
                                   WHERE FileId = @fileId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@fileId", fileId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sharedAccesses.Add(new SharedAcess
                                {
                                    ShareId = reader.GetInt32(0),
                                    FileId = reader.GetInt32(1),
                                    SharedByUserId = reader.GetInt32(2),
                                    SharedToUserId = reader.GetInt32(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving shared access: {ex.Message}", ex);
            }
            return sharedAccesses;
        }
    }
}
