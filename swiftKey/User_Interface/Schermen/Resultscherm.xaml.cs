

namespace User_Interface
{

	public partial class Resultscherm : ContentPage
	{
		public Resultscherm(int typeSpeed,string timeTaken, double accuracy, string enteredText, string targetText)
		{
			InitializeComponent();
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
			Wpm.Text=$"type snelheid:{typeSpeed}";
			Time.Text = $"Tijd:{timeTaken}";
			Ratio.Text = $"Goed/Fout ratio:{accuracy}%";
			Mistakes.Text = $"Fouten:{mistakes}";

		}
	}
}