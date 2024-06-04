using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using Data_Access;
using Business_Logic;

namespace User_Interface.Schermen
{
    public partial class SelecterenOefening : ContentPage
    {
        private User user;

        public SelecterenOefening(User user)
        {
            InitializeComponent();
            this.user = user;

            LoadTags();
            LoadDifficulties();


            TagPicker.SelectedIndexChanged += TagPicker_SelectedIndexChanged;
            SelectOefeningen.ItemTapped += SelectOefeningen_ItemTapped;
            DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                TagPicker.SelectedItem = "All";
                DifficultyPicker.SelectedItem = "Any Difficulty";
                LoadLevels();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to load levels on reappearing: " + ex.Message, "OK");
            }
        }

        private async void onHomeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(user));
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeaderboardScherm(user));
        }

        private async void onProfielButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfielScherm(user));
        }
        private async void onBrowseButtonClicked(object sender, EventArgs e)
        {
           
            await DisplayAlert("Info", "You are already on the browse page.", "OK");
        }

        private async void onLogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Loginscherm());
        }

        private void LoadTags()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    //Alle tags ophalen
                    var levels = levelHandler.GetLevels();
                    List<string> tags = new List<string>();

                    if (levels != null && levels.Count > 0)
                    {
                        foreach (var level in levels)
                        {
                            string[] tagsArray = level.Tags?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(tag => tag.Trim())
                                                             .ToArray();
                            if (tagsArray != null)
                            {
                                tags.AddRange(tagsArray);
                            }
                        }

                        tags = tags.Distinct().ToList();
                    }
                    //Voeg "All" toe als eerste item om die default te maken
                    tags.Insert(0, "All");
                    TagPicker.Items.Clear();
                    foreach (var tag in tags)
                    {
                        TagPicker.Items.Add(tag);
                    }
                }
            }
            catch (Exception ex)
            {
                //Toon een alert als het laden van de tags mislukt
                DisplayAlert("Error", "Failed to load tags: " + ex.Message, "OK");
            }
        }

        private void LoadDifficulties()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    //Voeg "Any Difficulty" toe als eerste item om die default te maken
                    DifficultyPicker.Items.Clear();
                    DifficultyPicker.Items.Add("Any Difficulty");

                    var levels = levelHandler.GetLevels();
                    List<string> difficulties = new List<string>();

                    //Voeg alle verschillende moeilijkheidsgraden toe aan de DifficultyPicker
                    if (levels != null && levels.Count > 0)
                    {
                        difficulties = levels.Select(level => level.Difficulty).Distinct().ToList();
                    }

                    foreach (var difficulty in difficulties)
                    {
                        DifficultyPicker.Items.Add(difficulty);
                    }
                }
            }
            catch (Exception ex)
            {
                //Toon een alert als het laden van de moeilijkheidsgraden mislukt
                DisplayAlert("Error", "Failed to load difficulties: " + ex.Message, "OK");
            }
        }

        private void LoadLevels()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    //Alle levels ophalen
                    var levels = levelHandler.GetLevels();
                    //Toon een alert als er geen levels gevonden zijn
                    if (levels == null || levels.Count == 0)
                    {
                        DisplayAlert("Error", "Geen levels gevonden.", "OK");
                        NoLevelsLabel.IsVisible = true;
                        SelectOefeningen.IsVisible = false;
                        return;
                    }

                    //Alle completed levels van de user ophalen
                    var completedLevels = LevelCompletionHandler.GetCompletedLevels(user.Id);

                    //Voor elk level de completion status instellen
                    foreach (var level in levels)
                    {
                        level.IsCompleted = completedLevels.Contains(level.LevelId);
                    }

                    //Update de ListView met de levels
                    UpdateListView(levels, false);
                }
            }
            catch (Exception ex)
            {
                //Toon een alert als het laden van de levels mislukt
                DisplayAlert("Error", "Er zijn geen levels gevonden. " + ex.Message, "OK");
                NoLevelsLabel.IsVisible = true;
                SelectOefeningen.IsVisible = false;
            }
        }

        private void FilterLevels()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    //Geselecteerde tag en moeilijkheidsgraad ophalen
                    var selectedTag = TagPicker.SelectedItem?.ToString();
                    var selectedDifficulty = DifficultyPicker.SelectedItem?.ToString();

                    var filteredLevels = levelHandler.GetLevels().Where(level =>
                        (selectedTag == "All" || (level.Tags?.Contains(selectedTag) == true)) &&
                        (selectedDifficulty == "Any Difficulty" || level.Difficulty == selectedDifficulty)
                    ).ToList();

                    //Alle completed levels van de user ophalen
                    var completedLevels = LevelCompletionHandler.GetCompletedLevels(user.Id);

                    //Voor elk level de completion status instellen
                    foreach (var level in filteredLevels)
                    {
                        level.IsCompleted = completedLevels.Contains(level.LevelId);
                    }

                    UpdateListView(filteredLevels, true);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to filter levels: " + ex.Message, "OK");
            }
        }

        private void TagPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterLevels();
        }

        private void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterLevels();
        }

        private async void SelectOefeningen_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                // Check if the tapped item is an Oefening object
                if (e.Item is Oefening selectedOefening)
                {
                    string levelName = selectedOefening.Name.Replace("Level ", ""); // level naam ophalen

                    //naar het oefenscherm navigeren als er een voorbeeldtekst is
                    if (!string.IsNullOrEmpty(selectedOefening.ExampleText))
                    {
                        // Navigeer naar het oefenscherm
                        await Navigation.PushAsync(new Oefenscherm(user, levelName, selectedOefening.ExampleText));
                    }
                    else
                    {
                        // Toon een alert als er geen voorbeeldtekst is
                        await DisplayAlert("Error", "Er is geen voorbeeldtekst gevonden voor dit level.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Toon een alert als er een fout optreedt bij het navigeren naar het oefenscherm
                await DisplayAlert("Error", "Er is een fout opgetreden bij het navigeren naar het oefenscherm: " + ex.Message, "OK");
            }
            finally
            {
                // Reset het selected item
                ((ListView)sender).SelectedItem = null;
            }
        }

        private void UpdateListView(List<LevelHandler.Level> levels, bool isFiltered = true)
        {
            try
            {
                //Als er geen levels zijn of de levels leeg zijn, toon dan een label en verberg de ListView
                if (levels == null || levels.Count == 0)
                {
                    NoLevelsLabel.IsVisible = true;
                    SelectOefeningen.IsVisible = false;
                }
                else
                {
                    //Als er levels zijn, toon dan de ListView en verberg de label
                    NoLevelsLabel.IsVisible = false;
                    SelectOefeningen.IsVisible = true;

                    //Maak een lijst van Oefening objecten van de levels
                    List<Oefening> oefeningen = levels.Select(level =>
                        new Oefening
                        {
                            Name = "Level " + level.LevelId,
                            Tags = level.Tags,
                            Difficulty = level.Difficulty,
                            Image = level.IsCompleted ? "✅" : "❌",
                            ExampleText = level.ExampleText
                        }).ToList();

                    SelectOefeningen.ItemsSource = oefeningen;
                }
            }
            catch (Exception ex)
            {
                //Toon een alert als het updaten van de ListView mislukt
                DisplayAlert("Error", "Failed to update ListView: " + ex.Message, "OK");
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Handle ImageButton click event
        }
    }
}
