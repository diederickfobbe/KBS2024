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
                Thread.Sleep(100);
                user.CalculateExercisesCount();
                Thread.Sleep(100);
                user.CalculateAverageWpm();
                Thread.Sleep(100); 
                UserNameLabel.Text = "Welkom " + user.Username + "!";
                Thread.Sleep(100); 
                AverageWpmLabel.Text = user.AverageWpm.ToString();
                Thread.Sleep(100);
                AverageAccuracyLabel.Text = $"{user.AverageAccuracy}%";
                Thread.Sleep(100); 
                ExercisesCountLabel.Text = user.ExercisesCount.ToString();
            }
            else
            {
                UserNameLabel.Text = "Welkom!";
            }
        }
    }
}
