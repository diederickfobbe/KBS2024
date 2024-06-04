using System.Diagnostics;
using System.Linq;
using Business_Logic;
using Data_Access;
using Plugin.Maui.Audio;

using SwiftKey_Logic;
using User_Interface.Schermen;


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

            // door alle labels heengaan
            for (int i = 0; i < labelList.Count; i++)
            {
                if (i < enteredText.Length)
                {
                    // kleur veranderen als de letter goed of fout is
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
                    // als de letter niet is ingevoerd, terug naar de default kleur
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
            // Splits de doeltekst en de ingevoerde tekst in karakters
            char[] targetChars = targetText.ToCharArray();
            char[] enteredChars = enteredText.ToCharArray();

            int correctCharCount = 0;

            // Vergelijk karakter voor karakter en tel alleen correct overgetypte karakters
            for (int i = 0; i < Math.Min(targetChars.Length, enteredChars.Length); i++)
            {
                if (targetChars[i] == enteredChars[i])
                {
                    correctCharCount++;
                }
            }

            // Bereken de tijd en typesnelheid (WPM) op basis van correct overgetypte karakters
            double timeTakenInSeconds = stopwatch.Elapsed.TotalSeconds;

            // Typen is meestal gemeten in woorden per minuut waarbij een woord gemiddeld uit 5 karakters bestaat
            int totalWordsTyped = enteredChars.Length / 5;
            double typingSpeed = (totalWordsTyped / timeTakenInSeconds) * 60;

            // Bereken nauwkeurigheid op basis van het totale aantal karakters in de doeltekst
            double accuracy = ((double)correctCharCount / targetChars.Length) * 100;

            // Toon de resultaten
            ResultsLabel.Text = $"Typesnelheid: {typingSpeed:F2} WPM\nNauwkeurigheid: {accuracy:F2}%";

            try
            {
                RegisterScoresHandler.RegisterScore(UserID, LevelID, (int)typingSpeed, accuracy);
            }
            catch (Exception ex)
            {
                // Errors worden getoond in een alert
                await DisplayAlert("Error", "An error occurred while registering the score: " + ex.Message, "OK");
                return;
            }

            // Navigeer naar het resultaatscherm
            await Navigation.PushAsync(new Resultaatscherm(user, (int)typingSpeed, TimerLabel.Text, accuracy, enteredText, targetText));
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
                    BackgroundColor = Colors.LightGray // Default kleur
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

