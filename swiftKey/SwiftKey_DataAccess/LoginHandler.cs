using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Data_Access
{
    public class LoginHandler
    {
        public static bool CheckLogin(string username, string hashedPassword)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        // Select query to retrieve user with matching username and hashed password
                        string selectQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username AND CONVERT(nvarchar(MAX), Password) = @password";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameters to the query
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", hashedPassword);

                            // Execute the query
                            int count = (int)cmd.ExecuteScalar();

                            // If count > 0, user exists with provided username and hashed password
                            return count > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
}
