using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace User_Interface.Schermen
{
    public partial class SelecterenOefening : ContentPage
    {
        List<string> tags = new List<string>() { "engels", "easy", "woord" };
        List<Label> TagLabels;
        public SelecterenOefening()
        {
            InitializeComponent();
            InitializeOefeningen();           
            SelectOefeningen.ItemTapped += SelectOefeningen_ItemTapped;
        }

        private void InitializeOefeningen()
        {
            List<Oefening> oefeningen = new List<Oefening>()
            {
                new Oefening{Name="game1", Difficulty="Hard", ImageLocation="swiftkey.png"},
                new Oefening{Name="game2", Difficulty="Easy",ImageLocation="swiftkey.png"},
                new Oefening{Name="game3", Difficulty="medium",ImageLocation="swiftkey.png"},
                new Oefening{Name="game4", Difficulty="medium",ImageLocation="ukimage.png"}
            };
            SelectOefeningen.ItemsSource = oefeningen;

        }


    

        private async void SelectOefeningen_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Oefening selectedOefening)
            {
                // Open new Oefenscherm with the selected Oefening's text
                await Navigation.PushAsync(new Oefenscherm(selectedOefening.Name));
            }
        }
    }


    public class Oefening
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string ImageLocation { get; set; }
    }
}