using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data_Access
{
    public class LevelCompletionHandler
    {
        public static HashSet<int> GetCompletedLevels(int userId)
        {
            var completedLevels = new HashSet<int>();

            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        // Query to get all completed levels for the user
                        string selectQuery = "SELECT level_id FROM UserLevelCompletion WHERE user_id = @userId";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    completedLevels.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to get completed levels: " + ex.Message);
                    }
                }
            }

            return completedLevels;
        }
    }
}
