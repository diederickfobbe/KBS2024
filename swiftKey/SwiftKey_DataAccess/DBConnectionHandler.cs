using System;
using System.Data.SqlClient;
using Renci.SshNet;

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
            string sqlUsername = "sa",
            string sqlPassword = "KBSgroep3",
            string initialCatalog = "SwiftKey")
        {
            // SSH connection setup
            sshClient = new SshClient(sshHost, sshUsername, sshPassword);
            sshClient.Connect();

            // Local port forwarding
            port = new ForwardedPortLocal("127.0.0.1", 1433, sqlHost, 1433);
            sshClient.AddForwardedPort(port);
            port.Start();

            // SQL connection setup 
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
            // Ensure all resources are properly disposed
            try
            {
                SqlConnection?.Close();
                port?.Stop();
                sshClient?.RemoveForwardedPort(port);
                sshClient?.Disconnect();
                sshClient?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error disposing SSH client: " + ex.Message);
            }
        }
    }
}
