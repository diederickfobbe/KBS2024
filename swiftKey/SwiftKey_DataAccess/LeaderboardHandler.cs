using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data_Access
{
    public class LeaderboardHandler
    {
        private DBConnectionHandler dbConnection;

        public LeaderboardHandler(DBConnectionHandler dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public class LeaderboardEntry
        {
            public int Rank { get; set; }
            public string Username { get; set; }
            public float Score { get; set; }
            public int UserID { get; set; }
        }

        public List<LeaderboardEntry> GetLeaderboard()
        {
            List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

            try
            {
                using (SqlCommand command = new SqlCommand(
                    @"SELECT ROW_NUMBER() OVER (ORDER BY ulc.score DESC) AS Rank,
                     u.username AS Username,
                     ulc.score AS Score,
                     u.id as UserID
              FROM dbo.UserLevelCompletion ulc
              INNER JOIN dbo.Users u ON ulc.user_id = u.id
              INNER JOIN dbo.Level l ON ulc.level_id = l.id
              ORDER BY Score DESC",
                    dbConnection.SqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rank = (int)reader.GetInt64(0); // Explicitly cast to int
                            string username = reader.GetString(1);
                            double score = reader.GetDouble(2); // Read as Double
                            int userID = (int)reader.GetInt32(3); // Convert to int

                            // Create LeaderboardEntry object and add to the list
                            LeaderboardEntry entry = new LeaderboardEntry
                            {
                                Rank = rank,
                                Username = username,
                                Score = (float)score, // Convert to float
                                UserID = userID
                            };
                            leaderboard.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving leaderboard: " + ex.Message);
                // Add more detailed logging or rethrow the exception as needed
                throw;
            }

            return leaderboard;
        }

    }
}
