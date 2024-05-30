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
                    // Check if the score already exists
                    using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM UserLevelCompletion WHERE user_id = @UserId AND level_id = @LevelId", connectionHandler.SqlConnection))
                    {
                        checkCmd.Parameters.AddWithValue("@UserId", userId);
                        checkCmd.Parameters.AddWithValue("@LevelId", levelId);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Update the existing record
                            using (var updateCmd = new SqlCommand("UPDATE UserLevelCompletion SET wpm = @Wpm, accuracy = @Accuracy WHERE user_id = @UserId AND level_id = @LevelId", connectionHandler.SqlConnection))
                            {
                                updateCmd.Parameters.AddWithValue("@Wpm", wpm);
                                updateCmd.Parameters.AddWithValue("@Accuracy", (decimal)accuracy);
                                updateCmd.Parameters.AddWithValue("@UserId", userId);
                                updateCmd.Parameters.AddWithValue("@LevelId", levelId);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Insert a new record
                            using (var insertCmd = new SqlCommand("INSERT INTO UserLevelCompletion (user_id, level_id, wpm, accuracy) VALUES (@UserId, @LevelId, @Wpm, @Accuracy)", connectionHandler.SqlConnection))
                            {
                                insertCmd.Parameters.AddWithValue("@UserId", userId);
                                insertCmd.Parameters.AddWithValue("@LevelId", levelId);
                                insertCmd.Parameters.AddWithValue("@Wpm", wpm);
                                insertCmd.Parameters.AddWithValue("@Accuracy", (decimal)accuracy);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
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
