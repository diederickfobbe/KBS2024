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
            // Retrieve levels from database using LevelHandler
            var levels = levelHandler.GetLevels();

            // Convert levels to Oefening objects
            List<Oefening> oefeningen = levels.Select(level =>
                new Oefening
                {
                    Name = "Level " + level.Id, // Assuming tags represent the name in your database
                    Difficulty = level.Difficulty,
                    ImageLocation = "", // Provide appropriate image location if available in the database
                    ExampleText = level.ExampleText
                }).ToList();

           

            // Bind levels to ListView
            SelectOefeningen.ItemsSource = oefeningen;
        }

        private async void SelectOefeningen_ItemTapped(object sender, ItemTappedEventArgs e)
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
                    DisplayAlert("Error", "The level is corrupted", "Dismiss");
                }
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
        public string ImageLocation { get; set; }
        public string ExampleText { get; set; }
    }
}
