using SafeBox.Infrastructure.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SafeBox.Infrastructure.Services
{
    /// <summary>
    /// Service for managing database connections and testing connectivity
    /// </summary>
    public class DatabaseConnectionService
    {
        /// <summary>
        /// Tests the database connection
        /// </summary>
        /// <returns>True if connection is successful</returns>
        public static bool TestConnection()
        {
            return DatabaseHelper.TestConnection();
        }

        /// <summary>
        /// Tests the database connection and shows a message box with the result
        /// </summary>
        public static void TestConnectionWithMessage()
        {
            string errorMessage;
            bool success = DatabaseHelper.TestConnection(out errorMessage);

            if (success)
            {
                MessageBox.Show(
                    "Database connection successful!\n\n" +
                    $"Connection verified.",
                    "Connection Test - Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Database connection failed!\n\n" +
                    $"Error: {errorMessage}\n\n" +
                    $"Please check:\n" +
                    $"1. SQL Server is running\n" +
                    $"2. Server name in DatabaseHelper.cs is correct\n" +
                    $"3. Database 'SafeBox' exists\n" +
                    $"4. Windows Authentication is enabled\n" +
                    $"5. Your user has access to the database",
                    "Connection Test - Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets a new database connection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return DatabaseHelper.CreateConnection();
        }
    }
}
