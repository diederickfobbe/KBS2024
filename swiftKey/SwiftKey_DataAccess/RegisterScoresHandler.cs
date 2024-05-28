using System;
using System.Data.SqlClient;

namespace Data_Access
{
    public static class RegisterScoresHandler
    {
        public static void RegisterScore(int userId, string levelId, int wpm, double accuracy)
        {
            using (var connectionHandler = new DBConnectionHandler())
            {
                try
                {
                    using (var cmd = new SqlCommand("INSERT INTO UserLevelCompletion (user_id, level_id, wpm, accuracy) VALUES (@UserId, @LevelId, @Wpm, @Accuracy)", connectionHandler.SqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@LevelId", levelId);
                        cmd.Parameters.AddWithValue("@Wpm", wpm);
                        cmd.Parameters.AddWithValue("@Accuracy", (decimal)accuracy);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while registering the score: " + ex.Message);
                }
            }
        }
    }
}
