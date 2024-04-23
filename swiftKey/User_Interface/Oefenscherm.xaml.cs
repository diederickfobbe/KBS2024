using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.Linq;
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
        }
        

        private void TextInputEntry_Completed(object sender, EventArgs e)
        {
            stopwatch.Stop();
            string enteredText = TextInputEntry.Text.Trim();

            // Calculate typing speed, accuracy, and display results
            CalculateAndDisplayResults(enteredText);

            // Clear input
            TextInputEntry.Text = "";

            // Generate new target text
            targetText = OefenschermMethods.GenerateNewTargetText();
            InstructionsLabel.Text = targetText;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TextInputEntry.Focus();
            stopwatch.Start();
        }

    }
}
