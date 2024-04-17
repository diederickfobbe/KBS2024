using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Linq;

namespace User_Interface
{
    public partial class Oefenscherm : ContentPage
    {
        private string targetText = "The quick brown fox jumps over the lazy dog.";
        private Stopwatch stopwatch = new Stopwatch();

        public Oefenscherm()
        {
            InitializeComponent();
            InstructionsLabel.Text = "Type the text below:\n" + targetText;
        }

        private void TextInputEntry_Completed(object sender, EventArgs e)
        {
            stopwatch.Stop();
            string enteredText = TextInputEntry.Text.Trim();
            int enteredWordCount = enteredText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int targetWordCount = targetText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;

            // Calculate time taken in minutes
            double timeTakenInMinutes = stopwatch.Elapsed.TotalMinutes;

            // Calculate typing speed in words per minute (WPM)
            int typingSpeed = (int)(enteredWordCount / timeTakenInMinutes);

            // Calculate accuracy
            int correctWords = targetText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Intersect(enteredText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries))
                                         .Count();
            double accuracy = (double)correctWords / targetWordCount * 100;

            // Display results
            ResultsLabel.Text = $"Typing speed: {typingSpeed} WPM\nAccuracy: {accuracy:F2}%";

            // Clear input
            TextInputEntry.Text = "";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TextInputEntry.Focus();
            stopwatch.Start();
        }
    }
}
