using Data_Access;
using Business_Logic;
using System.Collections.Generic;
using System.Linq;

namespace User_Interface.Schermen
{
    public partial class LeaderboardScherm : ContentPage
    {
        private LeaderboardHandler leaderboardHandler;
        private User user;
        private int SelectedLevelID; // Field to store the selected level ID



        public LeaderboardScherm(User user)
        {
            InitializeComponent();
            this.user = user;

            // Initialize the leaderboard handler with DB connection
            DBConnectionHandler dbConnection = new DBConnectionHandler();
            leaderboardHandler = new LeaderboardHandler(dbConnection);

            // By default, set "All levels" as selected
            SelectedLevelID = -1; // -1 or any value that indicates "All levels"

            // Fetch and display the normal leaderboard
            DisplayLeaderboard();

            // Initialize pickers
            InitializePickers();
        }

        private async void DisplayLeaderboard()
        {
            try
            {
                List<LeaderboardHandler.LeaderboardEntry> leaderboard;

                // Check if "All levels" are selected
                if (SelectedLevelID == -1)
                {
                    // Fetch normal leaderboard data from the database
                    leaderboard = leaderboardHandler.GetLeaderboard();
                }
                else
                {
                    // Fetch leaderboard data for the selected level from the database
                    leaderboard = leaderboardHandler.GetLeaderboardForLevel(SelectedLevelID);
                }

                // If "All levels" are selected, aggregate scores by user ID
                if (SelectedLevelID == -1)
                {
                    // Aggregate scores by UserID
                    var aggregatedLeaderboard = leaderboard
                        .GroupBy(entry => entry.UserID)
                        .Select(group => new LeaderboardHandler.LeaderboardEntry
                        {
                            Rank = group.First().Rank, // Assume the same rank for all entries of the same user
                            Username = group.First().Username,
                            Score = group.Sum(entry => entry.Score),
                            UserID = group.Key, // Use the UserID from the group
                            LevelID = -1 // Indicates aggregation for all levels
                        })
                        .OrderByDescending(entry => entry.Score)
                        .ToList();

                    leaderboard = aggregatedLeaderboard;
                }

                // Clear any existing items in the ListView
                LeaderboardListView.ItemsSource = null;

                // Update the ListView with the fetched leaderboard data
                LeaderboardListView.ItemsSource = leaderboard;
            }
            catch (Exception ex)
            {
                // Show an alert with the error message
                await DisplayAlert("Error", $"Error displaying leaderboard: {ex.Message}", "OK");
            }
        }


        private void InitializePickers()
        {
            // Fetch level IDs from the database
            var levelIDs = leaderboardHandler.GetLevelIDs();

            // Convert level IDs to strings
            var levelOptions = levelIDs.ConvertAll(id => id.ToString());

            // Insert "All levels" as the first item
            levelOptions.Insert(0, "All levels");

            // Populate the LevelPicker with the retrieved level options
            LevelPicker.ItemsSource = levelOptions;

            // By default, select "All levels"
            LevelPicker.SelectedIndex = 0;
        }

        private void LevelPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected level option
            string selectedOption = (string)LevelPicker.SelectedItem;

            // If "All levels" is selected, set SelectedLevelID to -1
            SelectedLevelID = selectedOption == "All levels" ? -1 : int.Parse(selectedOption);

            // Fetch and display the leaderboard for the selected level
            DisplayLeaderboard();
        }


        private async void onHomeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(user));
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
            // Already on the leaderboard page, do nothing
            await DisplayAlert("Info", "You are already on the leaderboard page.", "OK");
        }

        private async void onProfielButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfielScherm(user));
        }

        private async void onLogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Loginscherm());
        }
    }
}
