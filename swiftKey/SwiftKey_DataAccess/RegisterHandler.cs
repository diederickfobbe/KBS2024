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
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";

                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                        {
                            checkCmd.Parameters.AddWithValue("@email", email);

                            int existingEmailCount = (int)checkCmd.ExecuteScalar();

                            if (existingEmailCount > 0)
                            {
                                throw new Exception("Email is al in gebruik.");
                            }
                        }

                        string insertQuery = "INSERT INTO Users (Username, Email, Password) VALUES (@username, @email, @password)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@password", password);

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
