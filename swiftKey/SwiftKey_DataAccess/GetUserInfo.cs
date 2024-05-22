using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class GetUserInfo
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
        public static int GetUserIDByEmail(string email)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string selectQuery = "SELECT id FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";


                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameter to the query
                            cmd.Parameters.AddWithValue("@email", email);

                            // Execute the query and get the hashed password
                            object result = cmd.ExecuteScalar();

                            // Check if the result is not null
                            if (result != null)
                            {

                                return (int)result;


                            }
                            else
                            {
                                return 0; // Email address not found
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


        public static int GetAverageWPM(int id)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string selectQuery = "SELECT AVG(wpm) FROM userlevelcompletion WHERE user_id = @id";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameter to the query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Execute the query and get the average
                            object result = cmd.ExecuteScalar();

                            // Check if the result is not null
                            if (result != null && result != DBNull.Value)
                            {
                                return Convert.ToInt32(result);
                            }
                            else
                            {
                                return 0; // No WPM values found
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



        public static double GetAverageAccuracy(int id)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string selectQuery = "SELECT AVG(accuracy) FROM userlevelcompletion WHERE user_id = @id";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameter to the query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Execute the query and get the average
                            object result = cmd.ExecuteScalar();

                            // Check if the result is not null
                            if (result != null && result != DBNull.Value)
                            {
                                return Convert.ToDouble(result);
                            }
                            else
                            {
                                return 0; // No accuracy values found
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

        public static int GetExercisesCount(int id)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string selectQuery = "SELECT COUNT(*) FROM userlevelcompletion WHERE user_id = @id";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameter to the query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Execute the query and get the count
                            object result = cmd.ExecuteScalar();

                            // Check if the result is not null
                            if (result != null)
                            {
                                return Convert.ToInt32(result);
                            }
                            else
                            {
                                return 0; // No exercises found for the user
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
