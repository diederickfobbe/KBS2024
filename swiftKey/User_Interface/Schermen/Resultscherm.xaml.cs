

using Business_Logic;
using User_Interface.Schermen;
using Data_Access;

namespace User_Interface
{

	public partial class Resultaatscherm : ContentPage
	{
        private User user;
        public Resultaatscherm(User user, int typeSpeed,string timeTaken, double accuracy, string enteredText, string targetText)
		{
			InitializeComponent();
            this.user = user;
            //
            asignResults(user.Id, typeSpeed,  timeTaken,  accuracy, ResultaatschermMethods.calculateMistakes( enteredText,  targetText));

        }


		private void asignResults(int user_id, int typeSpeed, string timeTaken, double accuracy, int mistakes)
		{
			Wpm.Text=$"type snelheid:{typeSpeed} WPM";
			Tijd.Text = $"Tijd:{timeTaken}";
            accuracy = Math.Round(accuracy, 2);
            Ratio.Text = $"Goed/Fout ratio:{accuracy}%";
			Fouten.Text = $"Fouten:{mistakes}";
            
		}

        private void DoneButton_Clicked(object sender, EventArgs e)
        {            
            
            Shell.Current.GoToAsync("../..");
        }
    }
}