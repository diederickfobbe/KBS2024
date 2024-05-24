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
                user.CalculateAverageAccuracy();
                
                user.CalculateExercisesCount();
                
                user.CalculateAverageWpm();
                
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
