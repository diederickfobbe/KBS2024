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
            public int LevelID { get; set; }
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
                             u.id AS UserID,
                             ulc.level_id AS LevelID
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
                            int userID = reader.GetInt32(3);
                            int levelID = reader.GetInt32(4);

                            LeaderboardEntry entry = new LeaderboardEntry
                            {
                                Rank = rank,
                                Username = username,
                                Score = (float)score, // Convert to float
                                UserID = userID,
                                LevelID = levelID
                            };
                            leaderboard.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving leaderboard: " + ex.Message);
                throw;
            }

            return leaderboard;
        }

        public List<int> GetLevelIDs()
        {
            List<int> levelIDs = new List<int>();

            try
            {
                using (SqlCommand command = new SqlCommand(
                    @"SELECT DISTINCT level_id
              FROM dbo.UserLevelCompletion",
                    dbConnection.SqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            levelIDs.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving level IDs: " + ex.Message);
                throw;
            }

            return levelIDs;
        }

        public List<LeaderboardEntry> GetLeaderboardForLevel(int levelID)
        {
            List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

            try
            {
                using (SqlCommand command = new SqlCommand(
                    @"SELECT ROW_NUMBER() OVER (ORDER BY ulc.score DESC) AS Rank,
                             u.username AS Username,
                             ulc.score AS Score,
                             u.id AS UserID,
                             ulc.level_id AS LevelID
                      FROM dbo.UserLevelCompletion ulc
                      INNER JOIN dbo.Users u ON ulc.user_id = u.id
                      WHERE ulc.level_id = @LevelID
                      ORDER BY Score DESC",
                    dbConnection.SqlConnection))
                {
                    command.Parameters.AddWithValue("@LevelID", levelID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rank = (int)reader.GetInt64(0); // Explicitly cast to int
                            string username = reader.GetString(1);
                            double score = reader.GetDouble(2); // Read as Double
                            int userID = reader.GetInt32(3);
                            int fetchedLevelID = reader.GetInt32(4);

                            LeaderboardEntry entry = new LeaderboardEntry
                            {
                                Rank = rank,
                                Username = username,
                                Score = (float)score, // Convert to float
                                UserID = userID,
                                LevelID = fetchedLevelID
                            };
                            leaderboard.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving leaderboard for level: " + ex.Message);
                throw;
            }

            return leaderboard;
        }

    }
}
