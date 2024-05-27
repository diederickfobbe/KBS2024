using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Emit;

namespace Data_Access
{
    public class LevelHandler
    {
        private DBConnectionHandler dbConnection;

        public LevelHandler(DBConnectionHandler dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public class Level
        {
            public int Id { get; set; }
            public string Tags { get; set; }
            public string Difficulty { get; set; }
            public string ExampleText { get; set; }
            public bool IsCompleted { get; set; }
        }

        public List<Level> GetLevels()
        {
            List<Level> levels = new List<Level>();

            try
            {
                using (SqlCommand command = new SqlCommand("SELECT id, tags, difficulty, example_text FROM Level", dbConnection.SqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string tags = reader.GetString(1);
                            string difficulty = reader.GetString(2);
                            string exampleText = reader.IsDBNull(3) ? null : reader.GetString(3);

                            // Create Level object and add to the list
                            Level level = new Level
                            {
                                Id = id,
                                Tags = tags,
                                Difficulty = difficulty,
                                ExampleText = exampleText
                            };
                            levels.Add(level);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving levels: " + ex.Message);
            }

            return levels;
        }
    }
}
