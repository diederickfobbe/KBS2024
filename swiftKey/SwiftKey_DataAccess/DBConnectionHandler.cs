using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;
using System.Data.SqlClient;

using System.Threading.Tasks;

namespace Data_Access
{
    public class DBConnectionHandler : IDisposable
    {

        private SshClient sshClient;
        private ForwardedPortLocal port;

        public SqlConnection SqlConnection { get; private set; }

        public DBConnectionHandler(
            string sshHost = "145.44.233.8",
            string sshUsername = "student",
            string sshPassword = "groep 3",
            string sqlHost = "localhost",
            string sqlUsername = "sql1",
            string sqlPassword = "kbsgroep3",
            string initialCatalog = "SwiftKey")
        {
            // SSH-verbinding instellen
            sshClient = new SshClient(sshHost, sshUsername, sshPassword);
            sshClient.Connect();

            // Lokale poort doorsturen
            port = new ForwardedPortLocal("127.0.0.1", 1433, sqlHost, 1433);
            sshClient.AddForwardedPort(port);
            port.Start();

            // SQL-verbinding instellen
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "127.0.0.1",
                UserID = sqlUsername,
                Password = sqlPassword,
                InitialCatalog = initialCatalog
            };

            SqlConnection = new SqlConnection(builder.ConnectionString);
            SqlConnection.Open();
        }

        public void Dispose()
        {
            try
            {
                // close SSH- and SQL-connections
                SqlConnection?.Close();
                sshClient?.RemoveForwardedPort(port);
                port?.Stop();
            }
            finally
            {
                // Ensure SSH client is disconnected and disposed, even if there was an exception
                sshClient?.Disconnect();
                sshClient?.Dispose();
            }
        }
    }
}
