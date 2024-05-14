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
        }



        private void CalculateAndDisplayResults(string enteredText)
        {
            // Splits de doeltekst en de ingevoerde tekst in woorden
            char[] targetWords = targetText.ToCharArray();
            char[] enteredWords = enteredText.ToCharArray();

            int correctWordCount = 0;

            // Vergelijk woord voor woord en tel alleen correct overgetypte woorden
            for (int i = 0; i < Math.Min(targetWords.Length, enteredWords.Length); i++)
            {
                if (targetWords[i] == enteredWords[i])
                {
                    correctWordCount++;
                }
            }

            // Bereken de tijd en typesnelheid (WPM) alleen op basis van correct overgetypte woorden
            double timeTakenInMinutes = stopwatch.Elapsed.TotalMinutes;
            int typingSpeed = OefenschermMethods.CalculateTypingSpeed(correctWordCount, timeTakenInMinutes);
            

            // Bereken nauwkeurigheid op basis van het totale aantal woorden in de doeltekst
            double accuracy = ((double)correctWordCount / targetWords.Length) * 100;

            // Toon de resultaten
            ResultsLabel.Text = $"Typesnelheid: {typingSpeed} WPM\nNauwkeurigheid: {accuracy:F2}%";
            Navigation.PushAsync(new Resultscherm(typingSpeed, TimerLabel.Text, accuracy, enteredText, targetText));
            stopwatch.Reset();
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
        private void OnEntryLoaded(object sender, EventArgs e)
        {
            TextInputEntry.Focus();
        }

        protected override void OnAppearing()
        {
            
            base.OnAppearing();
            TextInputEntry.Focus();
            stopwatch.Start();
        }

    }
}

