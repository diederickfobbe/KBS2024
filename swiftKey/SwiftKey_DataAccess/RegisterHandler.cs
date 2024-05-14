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
                        // Insert query with parameters to prevent SQL injection
                        string insertQuery = "INSERT INTO Users (Username, Email, Password) VALUES (@username, @email, @password)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            // Add parameters to the query
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);


                            // Execute the query
                            int rowsAffected = cmd.ExecuteNonQuery();

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
