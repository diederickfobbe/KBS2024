using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace User_Interface.Schermen
{
    public partial class SelecterenOefening : ContentPage
    {
        public SelecterenOefening()
        {
            InitializeComponent();
            InitializeOefeningen();
        }

        private void InitializeOefeningen()
        {
            List<Oefening> Games = new List<Oefening>()
            {
                new Oefening{Name="game1", Difficulty="Hard", ImageLocation="swiftkey.png"},
                new Oefening{Name="game2", Difficulty="Easy",ImageLocation="ukimage.png"},
                new Oefening{Name="game3", Difficulty="medium",ImageLocation="ukimage.png"}
            };
            SelectOefeningen.ItemsSource = Games;
        }
    }

    public class Oefening
    {
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string ImageLocation { get; set; }
    }
}