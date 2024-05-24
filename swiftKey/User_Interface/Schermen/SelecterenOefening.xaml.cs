using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await Navigation.PushAsync(new HomePage());
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LeaderboardScherm());
        }

        private async void onProfielButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfielScherm(user));
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
                DisplayAlert("Error", "Failed to load tags: " + ex.Message, "OK");
            }
        }

        private void LoadDifficulties()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    DifficultyPicker.Items.Clear();
                    DifficultyPicker.Items.Add("Any Difficulty");

                    var levels = levelHandler.GetLevels();
                    List<string> difficulties = new List<string>();

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
                DisplayAlert("Error", "Failed to load difficulties: " + ex.Message, "OK");
            }
        }

        private void LoadLevels()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    var levels = levelHandler.GetLevels();
                    if (levels == null || levels.Count == 0)
                    {
                        DisplayAlert("Error", "Geen levels gevonden.", "OK");
                        NoLevelsLabel.IsVisible = true;
                        SelectOefeningen.IsVisible = false;
                        return;
                    }

                    // Initial load without filtering
                    UpdateListView(levels, false);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Er zijn geen levels gevonden. " + ex.Message, "OK");
                NoLevelsLabel.IsVisible = true;
                SelectOefeningen.IsVisible = false;
            }
        }

        private void UpdateListView(List<LevelHandler.Level> levels, bool isFiltered = true)
        {
            try
            {
                if (levels == null || levels.Count == 0)
                {
                    NoLevelsLabel.IsVisible = true;
                    SelectOefeningen.IsVisible = false;
                }
                else
                {
                    NoLevelsLabel.IsVisible = false;
                    SelectOefeningen.IsVisible = true;

                    List<Oefening> oefeningen = levels.Select(level =>
                        new Oefening
                        {
                            Name = "Level " + level.Id,
                            Tags = level.Tags,
                            Difficulty = level.Difficulty,
                            Image = "❌",
                            ExampleText = level.ExampleText
                        }).ToList();

                    SelectOefeningen.ItemsSource = oefeningen;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to update ListView: " + ex.Message, "OK");
            }
        }

        private void FilterLevels()
        {
            try
            {
                using (var levelHandler = new LevelHandler())
                {
                    var selectedTag = TagPicker.SelectedItem?.ToString();
                    var selectedDifficulty = DifficultyPicker.SelectedItem?.ToString();

                    var filteredLevels = levelHandler.GetLevels().Where(level =>
                        (selectedTag == "All" || (level.Tags?.Contains(selectedTag) == true)) &&
                        (selectedDifficulty == "Any Difficulty" || level.Difficulty == selectedDifficulty)
                    ).ToList();

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
                if (e.Item is Oefening selectedOefening)
                {
                    string levelName = selectedOefening.Name.Replace("Level ", ""); // Remove "Level " prefix

                    // Check if the example text is available
                    if (!string.IsNullOrEmpty(selectedOefening.ExampleText))
                    {
                        // Pass the user object, level name, and example text to the Oefenscherm constructor
                        await Navigation.PushAsync(new Oefenscherm(user, levelName, selectedOefening.ExampleText));
                    }
                    else
                    {
                        // Display an alert if the example text is not available
                        await DisplayAlert("Error", "Er is geen voorbeeldtekst gevonden voor dit level.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an alert for any unexpected errors
                await DisplayAlert("Error", "Er is een fout opgetreden bij het navigeren naar het oefenscherm: " + ex.Message, "OK");
            }
            finally
            {
                // Deselect the tapped item
                ((ListView)sender).SelectedItem = null;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Handle ImageButton click event
        }
    }

}
