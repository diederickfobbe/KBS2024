using Renci.SshNet;
using System;
using System.Data.SqlClient;

namespace Data_Access
{
    public class DBConnectionHandler : IDisposable
    {
        private SshClient sshClient;
        private ForwardedPortLocal port;
        private bool disposed = false;

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
            try
            {
                Console.WriteLine("Setting up SSH connection...");

                // SSH connection setup
                sshClient = new SshClient(sshHost, sshUsername, sshPassword);
                sshClient.Connect();

                Console.WriteLine("SSH connection established.");

                // Local port forwarding using a dynamic port
                port = new ForwardedPortLocal("127.0.0.1", "localhost", 1433);
                sshClient.AddForwardedPort(port);
                port.Start();

                Console.WriteLine($"Port forwarding started on local port: {port.BoundPort}");

                // SQL connection setup 
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = $"127.0.0.1,{port.BoundPort}", // Use the dynamically assigned port
                    UserID = sqlUsername,
                    Password = sqlPassword,
                    InitialCatalog = initialCatalog
                };

                SqlConnection = new SqlConnection(builder.ConnectionString);
                SqlConnection.Open();

                Console.WriteLine("SQL connection established.");
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Failed to establish SSH or SQL connection: " + ex.Message, ex);
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    // Close SQL connection
                    SqlConnection?.Close();

                    // Stop and dispose of port forwarding
                    port?.Stop();
                    sshClient?.RemoveForwardedPort(port);

                    // Disconnect and dispose of SSH client
                    sshClient?.Disconnect();
                    sshClient?.Dispose();

                    Console.WriteLine("Resources disposed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error disposing resources: " + ex.Message);
                }
                finally
                {
                    disposed = true;
                }
            }
        }

        ~DBConnectionHandler()
        {
            Dispose();
        }
    }
}
