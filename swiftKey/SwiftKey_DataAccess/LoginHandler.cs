using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class LoginHandler
    {
        private static SqlConnectionStringBuilder _builder;
        public static bool CheckLogin(string email, string password)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT password FROM Users WHERE email = @email";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPasswordFromDB = reader.GetString(0);
                            // Hash het ingevoerde wachtwoord met SHA256 en vergelijk met het gehashte wachtwoord in de database
                            string hashedPassword = password;
                            if (hashedPassword == hashedPasswordFromDB)
                            {
                                return true;
                            }
                        }
                        return false; // Gebruiker niet gevonden
                    }
                }
            }

        }

    }

}
