using System.Diagnostics;
using System.Linq;
using Business_Logic;
using Data_Access;
using Plugin.Maui.Audio;




//using Android.OS;
using SwiftKey_Logic;


namespace User_Interface
{
    public partial class Oefenscherm : ContentPage
    {
        private string targetText = "";
        private List<char> targetTextList;
        private User user;
        private int UserID;
        private string LevelID;
        private IAudioPlayer player;

        private Stopwatch stopwatch = new Stopwatch();

        public Oefenscherm(User user, string levelID, string oefeningText)
        {
            InitializeComponent();
            
            this.user = user;
            this.UserID = GetUserInfo.GetUserIDByEmail(user.Email);
            this.LevelID = levelID;
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
        
        private async void TextInputEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            player.Play();   
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
            if (TextInputEntry.Text != null)
            {
                stopwatch.Stop();
                string enteredText = TextInputEntry.Text.Trim();
                CalculateAndDisplayResults(enteredText);
            }
        }


        private async void CalculateAndDisplayResults(string enteredText)
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

            try
            {
                RegisterScoresHandler.RegisterScore(UserID, LevelID, typingSpeed, accuracy);
            }
            catch (Exception ex)
            {
                // Display an error message using DisplayAlert
                await DisplayAlert("Error", "An error occurred while registering the score: " + ex.Message, "OK");
                return; // Exit the method early
            }

            // Proceed with navigation
            await Navigation.PushAsync(new Resultaatscherm(user, typingSpeed, TimerLabel.Text, accuracy, enteredText, targetText));
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
                    FontSize = 19,
                    WidthRequest = 25,
                    HeightRequest = 30,
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

        //het inputveld wordt in focus gebracht
        private void OnEntryLoaded(object sender, EventArgs e)
        {
            TextInputEntry.Focus();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("letterclick.wav"));
            stopwatch.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            player.Dispose();
        }

    }
}

