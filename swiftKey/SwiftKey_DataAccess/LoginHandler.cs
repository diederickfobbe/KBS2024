using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
                        // Selecteer de query om zo het gehashde wachtwoord te pakken op basis van het emailadres
                        string selectQuery = "SELECT password FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";


                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Voeg een parameter toe aan de query
                            cmd.Parameters.AddWithValue("@email", email);

                            // Voer de query uit en pak het gehashde wachtwoord
                            object result = cmd.ExecuteScalar();

                            // Check of result niet null is
                            if (result != null)
                            {
                               
                                return result.ToString();
                                Debug.WriteLine(result.ToString());
                                
                            }
                            else
                            {
                                return null; // Email address niet gevonden
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
