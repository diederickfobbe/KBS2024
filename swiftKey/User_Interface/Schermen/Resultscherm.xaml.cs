

using Business_Logic;
using User_Interface.Schermen;

namespace User_Interface
{

	public partial class Resultscherm : ContentPage
	{
        private User user;
        public Resultscherm(User user, int typeSpeed,string timeTaken, double accuracy, string enteredText, string targetText)
		{
			InitializeComponent();
            this.user = user;
            asignResults( typeSpeed,  timeTaken,  accuracy, calculateMistakes( enteredText,  targetText));

        }

		private int calculateMistakes(string enteredText, string targetText) 
		{
            var a = targetText.ToCharArray();
            var b = enteredText.ToCharArray();
            int incorrectWords = 0;
            int smallerArray;
            if (a.Count() >= b.Count()) { smallerArray = b.Count(); } else { smallerArray = a.Count(); }
            for (int i = 0; i < smallerArray; i++)
            {

                if (a[i] != b[i]) { incorrectWords++; }
            }
           
            return incorrectWords;
        }
		private void asignResults(int typeSpeed, string timeTaken, double accuracy, int mistakes)
		{
			Wpm.Text=$"type snelheid:{typeSpeed} WPM";
			Tijd.Text = $"Tijd:{timeTaken}";
            accuracy = Math.Round(accuracy, 2);
            Ratio.Text = $"Goed/Fout ratio:{accuracy}%";
			Fouten.Text = $"Fouten:{mistakes}";

		}

        private void DoneButton_Clicked(object sender, EventArgs e)
        {
             Navigation.PushAsync(new SelecterenOefening(user));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}