using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Access; // Import your data access namespace

namespace User_Interface.Schermen
{
    public partial class SelecterenOefening : ContentPage
    {
        private DBConnectionHandler dbConnection;
        private LevelHandler levelHandler;

        public SelecterenOefening()
        {
            InitializeComponent();

            // Initialize database connection
            dbConnection = new DBConnectionHandler();

            // Initialize LevelHandler
            levelHandler = new LevelHandler(dbConnection);

            // Load levels into ListView
            LoadLevels();
            LoadTags();
            LoadDifficulties();

            TagPicker.SelectedIndexChanged += TagPicker_SelectedIndexChanged;
            SelectOefeningen.ItemTapped += SelectOefeningen_ItemTapped;
            DifficultyPicker.SelectedIndexChanged += DifficultyPicker_SelectedIndexChanged; // Attach event handler

            // Ensure default selections are set after page has been fully rendered
            this.Appearing += (sender, e) =>
            {
                TagPicker.SelectedItem = "All";
                DifficultyPicker.SelectedItem = "Any Difficulty";
            };
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
            await Navigation.PushAsync(new ProfielScherm());
        }
        private async void onLogoutButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Loginscherm());
        }


        private void LoadTags()
        {
            try
            {
                // Retrieve levels from database using LevelHandler
                var levels = levelHandler.GetLevels();

                // Create a list to hold the tags
                List<string> tags = new List<string>();

                if (levels != null && levels.Count > 0)
                {
                    // Get distinct tags
                    foreach (var level in levels)
                    {
                        // Split tags by comma and trim any leading or trailing whitespace
                        string[] tagsArray = level.Tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(tag => tag.Trim())
                                                         .ToArray();

                        // Add each individual tag to the list
                        foreach (var tag in tagsArray)
                        {
                            tags.Add(tag);
                        }
                    }

                    // Remove duplicates
                    tags = tags.Distinct().ToList();
                }

                // Add "All" option at the beginning
                tags.Insert(0, "All");

                // Add tags to Picker
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

        private async void DifficultyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedTag = TagPicker.SelectedItem?.ToString();
                var selectedDifficulty = DifficultyPicker.SelectedItem?.ToString();

                Console.WriteLine("Selected Tag: " + selectedTag);
                Console.WriteLine("Selected Difficulty: " + selectedDifficulty);

                if (selectedDifficulty == "Any Difficulty")
                {
                    if (selectedTag == "All")
                    {
                        // Show all levels regardless of tag
                        var allLevels = levelHandler.GetLevels();
                        UpdateListView(allLevels);
                    }
                    else
                    {
                        // Show levels with the specified tag
                        var filteredLevels = levelHandler.GetLevels()
                                                         .Where(level => level.Tags?.Contains(selectedTag) == true)
                                                         .ToList();
                        UpdateListView(filteredLevels);
                    }
                }
                else
                {
                    // Filter levels based on both selected tag and difficulty
                    var filteredLevels = levelHandler.GetLevels()
                                                     .Where(level => (selectedTag == null || level.Tags?.Contains(selectedTag) == true) &&
                                                                     (selectedDifficulty == null || level.Difficulty == selectedDifficulty))
                                                     .ToList();

                    if (filteredLevels.Any())
                    {
                        UpdateListView(filteredLevels);
                    }
                    else
                    {
                        DisplayAlert("Info", "No levels found for the selected tag and difficulty.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to handle difficulty selection: " + ex.Message, "OK");
            }
        }




        private async void TagPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedTag = TagPicker.SelectedItem?.ToString();
                var selectedDifficulty = DifficultyPicker.SelectedItem?.ToString();

                Console.WriteLine("Selected Tag: " + selectedTag);
                Console.WriteLine("Selected Difficulty: " + selectedDifficulty);

                if (selectedTag == "All")
                {
                    if (selectedDifficulty == "Any Difficulty")
                    {
                        // Show all levels regardless of tag
                        var allLevels = levelHandler.GetLevels();
                        UpdateListView(allLevels);
                    }
                    else
                    {
                        // Show levels with the specified difficulty
                        var filteredLevels = levelHandler.GetLevels()
                                                         .Where(level => level.Difficulty == selectedDifficulty)
                                                         .ToList();
                        UpdateListView(filteredLevels);
                    }
                }
                else
                {
                    // Filter levels based on both selected tag and difficulty
                    var filteredLevels = levelHandler.GetLevels()
                                                     .Where(level => (selectedDifficulty == null || level.Difficulty == selectedDifficulty) &&
                                                                     (selectedTag == null || level.Tags?.Contains(selectedTag) == true))
                                                     .ToList();

                    if (filteredLevels.Any())
                    {
                        UpdateListView(filteredLevels);
                    }
                    else
                    {
                        DisplayAlert("Info", "No levels found for the selected tag and difficulty.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to handle tag selection: " + ex.Message, "OK");
            }
        }



        private void UpdateListView(List<LevelHandler.Level> levels)
        {
            try
            {
                if (levels == null || levels.Count == 0)
                {
                    DisplayAlert("Info", "No levels found for the selected tag.", "OK");
                    return;
                }

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
            catch (Exception ex)
            {
                DisplayAlert("Error", "Failed to update ListView: " + ex.Message, "OK");
            }
        }



        private void LoadLevels()
        {
            try
            {
                // Retrieve levels from database using LevelHandler
                var levels = levelHandler.GetLevels();

                if (levels == null || levels.Count == 0)
                {
                    DisplayAlert("Error", "Geen levels gevonden.", "OK");
                    return;
                }

                // Convert levels to Oefening objects
                List<Oefening> oefeningen = levels.Select(level =>
                    new Oefening
                    {
                        Name = "Level " + level.Id,
                        Tags = level.Tags,
                        Difficulty = level.Difficulty,
                        Image = "❌",
                        ExampleText = level.ExampleText
                    }).ToList();

                // Bind levels to ListView
                SelectOefeningen.ItemsSource = oefeningen;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Er zijn geen levels gevonden. " + ex.Message, "OK");
            }
        }


        private async void SelectOefeningen_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item is Oefening selectedOefening)
                {
                    if (!string.IsNullOrEmpty(selectedOefening.ExampleText))
                    {
                        // Open new Oefenscherm with the selected Oefening's text
                        await Navigation.PushAsync(new Oefenscherm(selectedOefening.ExampleText));
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
        }

        private void LoadDifficulties()
        {
            try
            {
                // Add "Any Difficulty" option at the beginning
                DifficultyPicker.Items.Add("Any Difficulty");

                // Retrieve levels from database using LevelHandler
                var levels = levelHandler.GetLevels();

                // Create a list to hold the difficulty levels
                List<string> difficulties = new List<string>();

                if (levels != null && levels.Count > 0)
                {
                    // Get distinct difficulties
                    difficulties = levels.Select(level => level.Difficulty).Distinct().ToList();
                }

                // Add difficulties to Picker
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



        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Dispose the database connection
            dbConnection.Dispose();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
    }

    public class Oefening
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Tags { get; set; }
        public string Image { get; set; }
        public string ExampleText { get; set; }
    }
}
