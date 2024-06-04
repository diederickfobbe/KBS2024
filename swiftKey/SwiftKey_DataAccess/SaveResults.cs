using System;
using System.Data.SqlClient;

namespace Data_Access
{
    public class SaveResults
    {
        public static bool SaveResultsDB(int user_id, int level_id, DateTime completion_date, int wpm, decimal accuracy, float score)
        {
            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string insertQuery = "INSERT INTO UserLevelCompletion (user_id, level_id, completion_date, wpm, accuracy, score) VALUES (@user_id, @level_id, @completion_date, @wpm, @accuracy, @score)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            //parameters 
                            cmd.Parameters.AddWithValue("@user_id", user_id);
                            cmd.Parameters.AddWithValue("@level_id", level_id);
                            cmd.Parameters.AddWithValue("@completion_date", completion_date);
                            cmd.Parameters.AddWithValue("@wpm", wpm);
                            cmd.Parameters.AddWithValue("@accuracy", accuracy);
                            cmd.Parameters.AddWithValue("@score", score);

                            // Execute query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            // hoeveel rijen zijn aangepast
                            return rowsAffected > 0;
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
