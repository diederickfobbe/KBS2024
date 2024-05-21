using System.Diagnostics;
using System.Linq;

//using Android.OS;
using SwiftKey_Logic;


namespace User_Interface
{
    public partial class Oefenscherm : ContentPage
    {
        private string targetText = "";
        private List<char> targetTextList;


        private Stopwatch stopwatch = new Stopwatch();

        public Oefenscherm(string oefeningText)
        {
            InitializeComponent();
            targetText = oefeningText;
            targetTextList = targetText.ToList();
            InstructionsLabel.Text = targetText;
            Device.StartTimer(TimeSpan.FromSeconds(1), UpdateTimer);
            Build();
            TextInputEntry.TextChanged += TextInputEntry_TextChanged;
        }


        private bool UpdateTimer()
        {
            TimerLabel.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            return true; // Return true to keep the timer running.
        }
        
        private void TextInputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string enteredText = e.NewTextValue ?? "";
            string[] enteredWords = enteredText.Split(' ');

            for (int i = 0; i < labelList.Count; i++)
            {
                if (i < enteredWords.Length)
                {
                    // Change color based on whether the entered word matches the target word
                    if (labelList[i].Text.Trim() == enteredWords[i])
                    {
                        labelList[i].BackgroundColor = Colors.Green;
                    }
                    else
                    {
                        labelList[i].BackgroundColor = Colors.Red;
                    }
                }
                else
                {
                    // No word entered in this position yet, reset to default
                    labelList[i].BackgroundColor = Colors.LightGray;
                }
            }
        }


        
        private void TextInputEntry_Completed(object sender, EventArgs e)
        {   
            if (TextInputEntry.Text != null)
            {
                stopwatch.Stop();
                string enteredText = TextInputEntry.Text.Trim();
                CalculateAndDisplayResults(enteredText);
            }
        }



        private void CalculateAndDisplayResults(string enteredText)
        {
            string[] targetWords = targetText.Split(' ');
            string[] enteredWords = enteredText.Split(' ');

            int correctWordCount = 0;

            // Compare word for word and count only correctly typed words
            for (int i = 0; i < Math.Min(targetWords.Length, enteredWords.Length); i++)
            {
                if (targetWords[i] == enteredWords[i])
                {
                    correctWordCount++;
                }
            }

            // Calculate the time and typing speed (WPM) based on correctly typed words
            double timeTakenInMinutes = stopwatch.Elapsed.TotalMinutes;
            int typingSpeed = OefenschermMethods.CalculateTypingSpeed(correctWordCount, timeTakenInMinutes);

            // Calculate accuracy based on the total number of words in the target text
            double accuracy = ((double)correctWordCount / targetWords.Length) * 100;

            // Display the results
            ResultsLabel.Text = $"Typesnelheid: {typingSpeed} WPM\nNauwkeurigheid: {accuracy:F2}%";
            Navigation.PushAsync(new Resultscherm(typingSpeed, TimerLabel.Text, accuracy, enteredText, targetText));
            stopwatch.Reset();
        }



        private List<Label> labelList = new List<Label>();

        private void Build()
        {
            Sentence.Children.Clear();
            labelList.Clear();

            string[] words = targetText.Split(' ');

            foreach (string word in words)
            {
                Label wordLabel = new Label
                {
                    Text = word,
                    FontSize = 24,
                    Margin = new Thickness(2, 0),
                    BackgroundColor = Colors.LightGray, // Default color
                    TextColor = Colors.Black,
                    Padding = new Thickness(6, 4)
                };

                Sentence.Children.Add(wordLabel);
                labelList.Add(wordLabel);

                // Add a space after each word
                if (word != words.Last())
                {
                    Label spaceLabel = new Label
                    {
                        Text = " ",
                        FontSize = 24
                    };
                    Sentence.Children.Add(spaceLabel);
                    labelList.Add(spaceLabel);
                }
            }
        }


        //het inputveld wordt in focus gebracht
        private void OnEntryLoaded(object sender, EventArgs e)
        {
            TextInputEntry.Focus();
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            stopwatch.Start();
        }

    }
}

