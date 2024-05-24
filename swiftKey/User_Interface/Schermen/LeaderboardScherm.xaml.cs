
using Data_Access;
using Business_Logic;

namespace User_Interface.Schermen
{
    public partial class LeaderboardScherm : ContentPage
    {
        private LeaderboardHandler leaderboardHandler;
        private User user;

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



        // Add this method to your code-behind file to handle the SelectedIndexChanged event of the Picker
       

        private async void onHomeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Loginscherm());
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
            // Already on the leaderboard page, do nothing
            await DisplayAlert("Info", "You are already on the leaderboard page.", "OK");
        }

        private async void onProfielButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfielScherm( user));
        }

        private async void onLogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Loginscherm());
        }



    }
}
