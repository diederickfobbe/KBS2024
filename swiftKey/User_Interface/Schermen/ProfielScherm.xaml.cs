using Business_Logic;
using System.Text;
using static Data_Access.LevelHandler;

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


                UserNameLabel.Text = "Welkom " + user.Username + "!";
                
                AverageWpmLabel.Text = user.AverageWpm.ToString();
                
                AverageAccuracyLabel.Text = $"{user.AverageAccuracy}%";
                ExercisesCountLabel.Text = user.ExercisesCount.ToString() ;


                foreach (Level exercise in user.gemaakteOefeningen)
                {
                    CompletedExercisesLabel.Text += "\n";
                    CompletedExercisesLabel.Text += "Level: " + exercise.LevelId.ToString() + "         ";
                    CompletedExercisesLabel.Text += "Datum: " + exercise.CompletionDate.ToString() + "         ";
                    CompletedExercisesLabel.Text += "Woorden per minuut: " + exercise.Wpm.ToString() + "         ";
                    CompletedExercisesLabel.Text += "Accuracy: " + exercise.Accuracy.ToString() + "         ";
                    CompletedExercisesLabel.Text += "Score: " + exercise.Score.ToString() + "\n\n";
                }




            }
            else
            {
                UserNameLabel.Text = "Welkom!";
            }
        }
    }
}
