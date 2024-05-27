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
        private static DBConnectionHandler dbConnection;
        private LevelHandler levelHandler;
        private User user;

        public SelecterenOefening(User user)
        {
            InitializeComponent();
            this.user = user;

            InitializeHandlers();

            // Load tags and difficulties into respective controls
            LoadTags();
            LoadDifficulties();

            TagPicker.SelectedIndexChanged += TagPicker_SelectedIndexChanged;
            SelectOefeningen.ItemTapped += SelectOefeningen_ItemTapped;
            DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged;
        }

        private void InitializeHandlers()
        {
            // Initialize database connection if not already initialized
            if (dbConnection == null)
            {
                dbConnection = new DBConnectionHandler();
            }

            // Initialize LevelHandler
            levelHandler = new LevelHandler(dbConnection);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                InitializeHandlers(); // Reinitialize handlers to ensure fresh connection
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
            await Navigation.PushModalAsync(new Loginscherm());
        }

        private async void onLeaderboardButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LeaderboardScherm());
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
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to load tags: " + ex.Message, "OK");
            }
        }

        private void LoadDifficulties()
        {
            try
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
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to load difficulties: " + ex.Message, "OK");
            }
        }

        private void LoadLevels()
        {
            try
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
                            Image = level.IsCompleted ? "✔️" : "❌", // Checkmark if completed, otherwise cross
                            ExampleText = level.ExampleText,
                            IsCompleted = level.IsCompleted // Assuming you have this property in Level class
                        }).ToList();

                    SelectOefeningen.ItemsSource = oefeningen;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to update ListView: " + ex.Message, "OK");
            }
        }

        private void MarkExerciseAsCompleted(Oefening oefening)
        {
            // Find the corresponding level in LevelHandler and mark it as completed
            var level = levelHandler.GetLevels().FirstOrDefault(l => l.ExampleText == oefening.ExampleText);
            if (level != null)
            {
                level.IsCompleted = true;
                oefening.IsCompleted = true;
                oefening.Image = "✔️"; // Update the image to checkmark
                UpdateListView(levelHandler.GetLevels(), true); // Refresh the ListView
            }
        }

        private void FilterLevels()
        {
            try
            {
                var selectedTag = TagPicker.SelectedItem?.ToString();
                var selectedDifficulty = DifficultyPicker.SelectedItem?.ToString();

                var filteredLevels = levelHandler.GetLevels().Where(level =>
                    (selectedTag == "All" || (level.Tags?.Contains(selectedTag) == true)) &&
                    (selectedDifficulty == "Any Difficulty" || level.Difficulty == selectedDifficulty)
                ).ToList();

                UpdateListView(filteredLevels, true);
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
                    if (!string.IsNullOrEmpty(selectedOefening.ExampleText))
                    {
                        // Navigate to Oefenscherm and handle exercise completion
                        await Navigation.PushAsync(new Oefenscherm(user, selectedOefening.ExampleText));

                        // Mark the exercise as completed after completion
                        MarkExerciseAsCompleted(selectedOefening);
                    }
                    else
                    {
                        DisplayAlert("Error", "Er is geen voorbeeldtekst gevonden voor dit level.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "An error occurred while navigating to Oefenscherm: " + ex.Message, "OK");
            }
            finally
            {
                ((ListView)sender).SelectedItem = null; // Ensure the item is deselected
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            dbConnection.Dispose();
            dbConnection = null; // Ensure the connection is properly disposed of
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Handle ImageButton click event
        }
    }

    public class Oefening
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Tags { get; set; }
        public string Image { get; set; }
        public string ExampleText { get; set; }
        public bool IsCompleted { get; set; }
    }
}
