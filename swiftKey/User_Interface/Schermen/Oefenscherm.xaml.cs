using System.Diagnostics;
//using Android.OS;
using SwiftKey_Logic;

namespace User_Interface
{
    public partial class Oefenscherm : ContentPage
    {
        private string targetText = "";
        private Stopwatch stopwatch = new Stopwatch();

        public Oefenscherm()
        {
            InitializeComponent();
            targetText = OefenschermMethods.GenerateNewTargetText();
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

            if (!stopwatch.IsRunning) 
            {
                stopwatch.Start(); // Restart the stopwatch for a new measurement
            }
            // Iterate through all labels
            for (int i = 0; i < labelList.Count; i++)
            {
                if (i < enteredText.Length)
                {
                    // Change color based on whether the entered character matches the target
                    if (enteredText[i] == targetText[i])
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
                    // No character entered in this position yet, reset to default
                    labelList[i].BackgroundColor = Colors.LightGray;
                }
            }
        }


        
        private void TextInputEntry_Completed(object sender, EventArgs e)
        {
            stopwatch.Stop();
            string enteredText = TextInputEntry.Text.Trim();
            CalculateAndDisplayResults(enteredText);

            TextInputEntry.Text = "";
            targetText = OefenschermMethods.GenerateNewTargetText();
            InstructionsLabel.Text = targetText;
            

            Build(); // Rebuild the labels for the new text

            stopwatch.Reset();
        }



        private void CalculateAndDisplayResults(string enteredText)
        {
            int enteredWordCount = OefenschermMethods.GetWordCount(enteredText);
            int targetWordCount = OefenschermMethods.GetWordCount(targetText);

            // Calculate time taken in minutes
            double timeTakenInMinutes = stopwatch.Elapsed.TotalMinutes;

            // Calculate typing speed in words per minute (WPM)
            int typingSpeed = OefenschermMethods.CalculateTypingSpeed(enteredWordCount, timeTakenInMinutes);

            // Calculate accuracy
            double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

            // Display results
            ResultsLabel.Text = $"Typesnelheid: {typingSpeed} WPM\nNauwkeurigheid: {accuracy:F2}%";
            Navigation.PushAsync(new Resultscherm(typingSpeed, TimerLabel.Text,accuracy,enteredText, targetText));
        }

        private List<Label> labelList = new List<Label>();

        private void Build()
        {
            Sentence.Children.Clear();
            labelList.Clear();

            foreach (char letter in targetText)
            {
                Label letterLabel = new Label
                {
                    Text = letter.ToString(),
                    FontSize = 36,
                    WidthRequest = 40,
                    HeightRequest = 60,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(6, 4, 6, 4),
                    TextColor = Colors.Black,
                    Margin = 2,
                    BackgroundColor = Colors.LightGray // Default color
                };
                Sentence.Children.Add(letterLabel);
                labelList.Add(letterLabel);
            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            TextInputEntry.Focus();
            stopwatch.Start();
        }

    }
}

