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

            SelectOefeningen.ItemTapped += SelectOefeningen_ItemTapped;
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
                        ImageLocation = "swiftkey", 
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Dispose the database connection
            dbConnection.Dispose();
        }
    }

    public class Oefening
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Tags { get; set; }
        public string ImageLocation { get; set; }
        public string ExampleText { get; set; }
    }
}
