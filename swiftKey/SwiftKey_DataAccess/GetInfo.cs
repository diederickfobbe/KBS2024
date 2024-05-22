using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class GetInfo
    {

        public static string GetUserNameFromDB(string email)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {

                        // Select query to get the hashed password based on email address
                        /*string selectQuery = "SELECT password FROM Users WHERE email = @email";*/
                        // Select query to get the hashed password based on email address
                        string selectQuery = "SELECT username FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";


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
                                Debug.WriteLine(result.ToString());

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
    }
}
