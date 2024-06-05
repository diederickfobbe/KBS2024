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
        private int SelectedLevelID;



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
            var levelIDs = leaderboardHandler.GetLevelIDs();

            var levelOptions = levelIDs.ConvertAll(id => id.ToString());

            levelOptions.Insert(0, "All levels");

            LevelPicker.ItemsSource = levelOptions;

            LevelPicker.SelectedIndex = 0;
        }

        private void LevelPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = (string)LevelPicker.SelectedItem;

            SelectedLevelID = selectedOption == "All levels" ? -1 : int.Parse(selectedOption);

            DisplayLeaderboard();
        }


        private async void onHomeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(user));
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
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
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }
}
