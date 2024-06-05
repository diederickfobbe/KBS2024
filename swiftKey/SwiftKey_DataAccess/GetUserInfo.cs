using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data_Access.LevelHandler;

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
                        // Selecteer de query om zo het gehashde wachtwoord te pakken op basis van het emailadres
                        string selectQuery = "SELECT username FROM Users WHERE CAST(email AS NVARCHAR(MAX)) = @email";


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
                            // Voeg een parameter toe aan de query
                            cmd.Parameters.AddWithValue("@email", email);

                            // Voer de query uit en pak het gehashde wachtwoord
                            object result = cmd.ExecuteScalar();

                            // Check of result niet null is
                            if (result != null)
                            {

                                return (int)result;


                            }
                            else
                            {
                                return 0; // Email address niet gevonden
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
                            // Voeg een parameter toe aan de query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Voer de query uit en neem het gemiddelde
                            object result = cmd.ExecuteScalar();

                            // Check of result niet null is
                            if (result != null && result != DBNull.Value)
                            {
                                return Convert.ToInt32(result);
                            }
                            else
                            {
                                return 0; // Geen WPM values gevonden
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
                            // Voeg een parameter toe aan de query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Voer de query uit en neem het gemiddelde
                            object result = cmd.ExecuteScalar();

                            // Check of result niet null is
                            if (result != null && result != DBNull.Value)
                            {
                                return Convert.ToDouble(result);
                            }
                            else
                            {
                                return 0; // Geen accuracy values gevonden
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
                            // Voeg een parameter toe aan de query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Voer de query uit en pak het aantal gemaakte oefeningen
                            object result = cmd.ExecuteScalar();

                            // Check of result niet null is
                            if (result != null)
                            {
                                return Convert.ToInt32(result);
                            }
                            else
                            {
                                return 0; // Geen oefeningen gevonden voor de gebruiker
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



        public static List<Level> GetGebruikerVoltooideLevels(int id)
        {
            List<Level> voltooideLevels = new List<Level>();

            using (var connectionManager = new DBConnectionHandler())
            {
                using (SqlConnection connection = connectionManager.SqlConnection)
                {
                    try
                    {
                        string selectQuery = "SELECT * FROM userlevelcompletion WHERE user_id = @id";

                        using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                        {
                            // Parameter toevoegen aan de query
                            cmd.Parameters.AddWithValue("@id", id);

                            // Verbinding openen

                            // Query uitvoeren en gegevens lezen
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Gegevens omzetten in Level-objecten
                                    Level level = new Level
                                    {
                                        UserId = (int)reader["user_id"],
                                        LevelId = (int)reader["level_id"],
                                        CompletionDate = (DateTime)reader["completion_date"],
                                        Wpm = (int)reader["wpm"],
                                        Accuracy = Convert.ToDouble(reader["accuracy"]), // Use Convert.ToDouble
                                        Score = Convert.ToDouble(reader["score"]), // Use Convert.ToDouble
                                        Id = (int)reader["id"]
                                    };

                                    // Object toevoegen aan de lijst
                                    voltooideLevels.Add(level);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Algemene foutafhandeling
                        throw new Exception(ex.Message);
                    }
                }
            }
            return voltooideLevels;
        }
    }
}
