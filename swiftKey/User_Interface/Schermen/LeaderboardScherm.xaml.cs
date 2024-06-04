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

            DBConnectionHandler dbConnection = new DBConnectionHandler();
            leaderboardHandler = new LeaderboardHandler(dbConnection);

            SelectedLevelID = -1; //-1 betekend alle levels

            DisplayLeaderboard();
            InitializePickers();
        }

        private async void DisplayLeaderboard()
        {
            try
            {
                List<LeaderboardHandler.LeaderboardEntry> leaderboard;

                if (SelectedLevelID == -1)
                {
                    leaderboard = leaderboardHandler.GetLeaderboard();
                }
                else
                {
                    leaderboard = leaderboardHandler.GetLeaderboardForLevel(SelectedLevelID);
                }

                if (SelectedLevelID == -1)
                {
                    var aggregatedLeaderboard = leaderboard
                        .GroupBy(entry => entry.UserID)
                        .Select(group => new LeaderboardHandler.LeaderboardEntry
                        {
                            Username = group.First().Username,
                            Score = group.Sum(entry => entry.Score),
                            UserID = group.Key,
                            LevelID = -1
                        })
                        .OrderByDescending(entry => entry.Score)
                        .ToList();

                    for (int i = 0; i < aggregatedLeaderboard.Count; i++)
                    {
                        aggregatedLeaderboard[i].Rank = i + 1;
                    }

                    leaderboard = aggregatedLeaderboard;
                }

                LeaderboardListView.ItemsSource = null;
                LeaderboardListView.ItemsSource = leaderboard;
            }
            catch (Exception ex)
            {
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

        private async void onBrowseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelecterenOefening(user));
        }

        private async void onLogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Loginscherm());
        }
    }
}
