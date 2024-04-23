using System.Diagnostics;
//using Android.OS;
using SwiftKey_Logic;

namespace User_Interface
{
    public partial class Oefenscherm : ContentPage
    {
        private string targetText = "";
        private List<char> targetTextList;


        private Stopwatch stopwatch = new Stopwatch();

        public Oefenscherm()
        {
            InitializeComponent();
            targetText = OefenschermMethods.GenerateNewTargetText();
            targetTextList = new List<char>();
            targetTextList = targetText.ToList();
            InstructionsLabel.Text = targetText;
            Device.StartTimer(TimeSpan.FromSeconds(1), UpdateTimer);
            Build();
        }

        private bool UpdateTimer()
        {
            TimerLabel.Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            return true; // Return true to keep the timer running.
        }

        private void TextInputEntry_Completed(object sender, EventArgs e)
        {
            stopwatch.Stop();

            string enteredText = TextInputEntry.Text.Trim();
            CalculateAndDisplayResults(enteredText);

            //hello
            TextInputEntry.Text = "";
            targetText = OefenschermMethods.GenerateNewTargetText();
            InstructionsLabel.Text = targetText;

            stopwatch.Reset();
            stopwatch.Start(); // Restart the stopwatch for a new measurement
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
        }

        private void Build()
        {
            for (int i = 0; i < targetTextList.Count; i++)
            {
                Label letterLabel = new Label
                {
                    Text = targetTextList[i].ToString(),
                    FontSize = 36,
                    WidthRequest = 40,
                    HeightRequest = 60,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(6, 4, 6, 4),
                    //BackgroundColor = Color.FromArgb("#F1F1F1"),
                    BackgroundColor = Color.FromArgb("#66FF66"),
                    TextColor = Colors.Black,
                    Margin = 2
                };
                Sentence.Children.Add(letterLabel);
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

