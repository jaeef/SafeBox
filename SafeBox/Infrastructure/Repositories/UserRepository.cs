using SafeBox.Domain.Entities;
using SafeBox.Application.Interfaces;
using SafeBox.Application.Interfaces.Repositories;
using SafeBox.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SafeBox.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = DatabaseHelper.ConnectionString;
        }

        public void UpdateLastLogin(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "UPDATE Users SET LastLoginDate = @lastLoginDate WHERE UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@lastLoginDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating last login: {ex.Message}", ex);
            }
        }

        public void Add(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO Users (Username, Email, Password_Hash, Role_ID, Status, CreatedDate) 
                                   VALUES (@username, @email, @passwordHash, @roleId, @status, @createdDate);
                                   SELECT CAST(SCOPE_IDENTITY() as int);";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@email", user.Email);
                        cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@roleId", user.RoleId);
                        cmd.Parameters.AddWithValue("@status", user.Status == "Active" ? 1 : 0);
                        cmd.Parameters.AddWithValue("@createdDate", DateTime.Now);

                        user.UserId = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }

        public User GetById(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users WHERE UserId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"]),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user: {ex.Message}", ex);
            }
            return null;
        }

        public User GetByUsername(string username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users WHERE Username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"]),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user: {ex.Message}", ex);
            }
            return null;
        }

        public User GetByEmail(string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users WHERE Email = @email";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"]),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user by email: {ex.Message}", ex);
            }
            return null;
        }

        public bool UserExists(string username, string email)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @username OR Email = @email";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking if user exists: {ex.Message}", ex);
            }
        }

        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users 
                                   ORDER BY UserId DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"]),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}", ex);
            }
            return users;
        }

        public IEnumerable<User> Search(string searchTerm)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users 
                                   WHERE Username LIKE @search
                                   ORDER BY UserId DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchTerm}%");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"]),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching users: {ex.Message}", ex);
            }
            return users;
        }

        public void Update(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"UPDATE Users 
                                   SET Username = @username, Email = @email, Password_Hash = @passwordHash, Status = @status
                                   WHERE UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", user.UserId);
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@email", user.Email);
                        cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@status", user.Status == "Active" ? 1 : 0);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}", ex);
            }
        }

        public void ActivateUser(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "UPDATE Users SET Status = 1 WHERE UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error activating user: {ex.Message}", ex);
            }
        }

        public void DeactivateUser(int userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "UPDATE Users SET Status = 0 WHERE UserId = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating user: {ex.Message}", ex);
            }
        }

        public int GetActiveUserCount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Status = 1";

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

        public int GetInactiveUserCount()
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

        public int GetTotalUserCount()
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

        public bool IsEmailTaken(string email, int excludeUserId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Email = @email AND UserId != @excludeUserId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@excludeUserId", excludeUserId);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return true;
            }
        }

        public IEnumerable<User> GetRecent(int count)
        {
            List<User> users = new List<User>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT TOP (@count) UserId, Username, Email, Password_Hash, Role_ID, Status, CreatedDate, LastLoginDate 
                                   FROM Users 
                                   ORDER BY UserId DESC";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@count", count);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    PasswordHash = reader["Password_Hash"] == DBNull.Value ? null : (byte[])reader["Password_Hash"],
                                    RoleId = reader["Role_ID"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Role_ID"]),
                                    Status = reader["Status"] == DBNull.Value ? "Active" : (Convert.ToInt32(reader["Status"]) == 1 ? "Active" : "Inactive"),
                                    LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginDate"]),
                                    CreatedDate = reader["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreatedDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving recent users: {ex.Message}", ex);
            }
            return users;
        }

        public void UpdatePassword(int userId, byte[] passwordHash)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE Users SET Password_Hash = @passwordHash WHERE UserId = @userId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public byte[] GetPasswordHash(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "SELECT Password_Hash FROM Users WHERE UserId = @userId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    object result = cmd.ExecuteScalar();
                    return result == DBNull.Value ? null : (byte[])result;
                }
            }
        }

        public Admin GetAdminByUsername(string username)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"SELECT admin_id, admin_username, email, password_hash, status, created_at 
                                   FROM Admin WHERE admin_username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Admin
                                {
                                    AdminId = Convert.ToInt32(reader["admin_id"]),
                                    AdminUsername = reader["admin_username"].ToString(),
                                    Email = reader["email"].ToString(),
                                    PasswordHash = reader["password_hash"].ToString(),
                                    Status = Convert.ToBoolean(reader["status"]),
                                    CreatedAt = Convert.ToDateTime(reader["created_at"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving admin: {ex.Message}", ex);
            }
            return null;
        }

        public void UpdateAdminEmail(int adminId, string newEmail)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE Admin SET email = @email WHERE admin_id = @adminId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", newEmail);
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAdminPassword(int adminId, string newPasswordHash)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string query = "UPDATE Admin SET password_hash = @passwordHash WHERE admin_id = @adminId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@passwordHash", newPasswordHash);
                    cmd.Parameters.AddWithValue("@adminId", adminId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

