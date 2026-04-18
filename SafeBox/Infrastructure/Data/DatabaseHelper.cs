using System;
using System.Data.SqlClient;

namespace SafeBox.Infrastructure.Data
{
    public static class DatabaseHelper
    {
        public static string ConnectionString
        {
            get
            {
                return @"Data Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=SafeBox;Integrated Security=True;TrustServerCertificate=True";
            }
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection con = CreateConnection())
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection test failed: {ex.Message}");
                return false;
            }
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (SqlConnection con = CreateConnection())
                {
                    con.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                System.Diagnostics.Debug.WriteLine($"Database connection test failed: {ex.Message}");
                return false;
            }
        }
    }
}
