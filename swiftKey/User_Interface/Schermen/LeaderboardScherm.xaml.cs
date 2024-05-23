using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using Data_Access;

namespace User_Interface.Schermen
{
    public partial class LeaderboardScherm : ContentPage
    {
        private LeaderboardHandler leaderboardHandler;

        public LeaderboardScherm()
        {
            InitializeComponent();

            // Initialize the leaderboard handler with DB connection
            DBConnectionHandler dbConnection = new DBConnectionHandler(); // Initialize your DB connection handler here
            leaderboardHandler = new LeaderboardHandler(dbConnection);

            // Fetch and display leaderboard data
            DisplayLeaderboard();
        }

        private async void DisplayLeaderboard()
        {
            try
            {
                // Fetch leaderboard data from the database
                var leaderboard = leaderboardHandler.GetLeaderboard();

                // Show an alert with the number of entries fetched
                await DisplayAlert("Info", $"Fetched {leaderboard.Count} leaderboard entries.", "OK");

                // Clear any existing UI elements
                MainLayout.Children.Clear();

                // Create UI elements for each leaderboard entry
                foreach (var entry in leaderboard)
                {
                    var rankLabel = new Label { Text = $"Rank: {entry.Rank}" };
                    var usernameLabel = new Label { Text = $"Username: {entry.Username}" };
                    var scoreLabel = new Label { Text = $"Score: {entry.Score}" };

                    // Add UI elements to the main layout
                    MainLayout.Children.Add(rankLabel);
                    MainLayout.Children.Add(usernameLabel);
                    MainLayout.Children.Add(scoreLabel);

                    // Add some spacing between entries
                    MainLayout.Children.Add(new BoxView { HeightRequest = 10, Color = Colors.Transparent });
                }

                if (leaderboard.Count == 0)
                {
                    MainLayout.Children.Add(new Label { Text = "No leaderboard entries found." });
                }
            }
            catch (Exception ex)
            {
                // Show an alert with the error message
                await DisplayAlert("Error", $"Error displaying leaderboard: {ex.Message}", "OK");
                MainLayout.Children.Add(new Label { Text = $"Error: {ex.Message}" });
            }
        }
    }
}
