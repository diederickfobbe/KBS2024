using Business_Logic;

namespace User_Interface.Schermen
{
    public partial class ProfielScherm : ContentPage
    {
        private User user;

        public ProfielScherm(User user)
        {
            InitializeComponent();
            this.user = user;
            SetUpUserProfile();
        }

        private void SetUpUserProfile()
        {
            // Controleer of de gebruiker niet null is om NullReferenceException te voorkomen
            if (user != null)
            {
                
                user.RefreshStats();

/*                int exerciseCount = Data_Access.GetUserInfo.GetExercisesCount(5);

                // Toon een alert met het aantal oefeningen
                DisplayAlert("Mooi", exerciseCount.ToString(), "OK");*/

                UserNameLabel.Text = "Welkom " + user.Username + "!";
                
                AverageWpmLabel.Text = user.AverageWpm.ToString();
                
                AverageAccuracyLabel.Text = $"{user.AverageAccuracy}%";
                 
                ExercisesCountLabel.Text = user.ExercisesCount.ToString();
               
            }
            else
            {
                UserNameLabel.Text = "Welkom!";
            }
        }
    }
}
