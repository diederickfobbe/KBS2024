using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Data_Access
{
    public class LoginHandler
    {

        public static string GetPasswordFromDB(string email)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {

                        // Select query to get the hashed password based on email address
                        string selectQuery = "SELECT Password FROM Users WHERE email = @email";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameter to the query
                            cmd.Parameters.AddWithValue("@email", email);

                            // Execute the query and get the hashed password
                            object result = cmd.ExecuteScalar();

                            // Check if the result is not null
                            if (result != null)
                            {
                                return result.ToString();
                            }
                            else
                            {
                                return null; // Email address not found
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static bool CheckLogin(string email, string hashedPassword)
        {
            string dbPassword = GetPasswordFromDB (email);
            if(hashedPassword == dbPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
