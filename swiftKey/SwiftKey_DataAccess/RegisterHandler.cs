using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Data_Access
{
    public class RegisterHandler
    {


        public static bool RegisterUser(string username, string email, string password)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        // Check if the email already exists in the database
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";

                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                        {
                            // Add parameter to the query
                            checkCmd.Parameters.AddWithValue("@email", email);

                            // Execute the query to count rows with the given email
                            int existingEmailCount = (int)checkCmd.ExecuteScalar();

                            // If the email already exists, throw an exception
                            if (existingEmailCount > 0)
                            {
                                throw new Exception("Email is al in gebruik.");
                            }
                        }

                        // If the email doesn't exist, proceed with user registration
                        string insertQuery = "INSERT INTO Users (Username, Email, Password) VALUES (@username, @email, @password)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            // Add parameters to the query
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);

                            // Execute the query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // If rows were affected, registration was successful
                            if (rowsAffected > 0)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                return false;
            }
        }





    }
}
