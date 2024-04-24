using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Data_Access
{
    public class RegisterUser
    {
        // Connection string naar jouw SQL Server-database
        
        private static SqlConnectionStringBuilder _builder;
        public RegisterUser()
        {
            try
            {
                _builder = new SqlConnectionStringBuilder();
                _builder.DataSource = "(localdb)\\MSSQLLocalDB";
                _builder.IntegratedSecurity = true;
                _builder.ConnectTimeout = 30;
                _builder.UserID = "Diederick\\Diederick Fobbe";
                _builder.Password = "";
                _builder.InitialCatalog = "SwiftKey";
                _builder.Encrypt = false;
                _builder.TrustServerCertificate = false;
                _builder.ApplicationIntent = ApplicationIntent.ReadWrite;
                _builder.MultiSubnetFailover = false;
            }
            catch (Exception)
            {
                // ignored
            }
        }
        public static void InsertUser(string username, string email, string password)
        {
            using (var connection = new SqlConnection(_builder.ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Users (username, email, password) VALUES (@username, @email, @password)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    /*command.Parameters.AddWithValue("@firstname", firstname);*/
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public class LoginChecks
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
                                string hashedPassword = RegisterChecks.HashPassword(password);
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
}
