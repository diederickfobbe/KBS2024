using System;
using System.Data.SqlClient;

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

    
    }
}
