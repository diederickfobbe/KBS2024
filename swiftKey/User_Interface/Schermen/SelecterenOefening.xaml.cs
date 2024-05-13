using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Interface.Schermen;

public partial class SelecterenOefening : ContentPage {

    public SelecterenOefening()
    {
        InitializeComponent();


        List<Game> Games = new List<Game>()
        {
            new Game{Name="game1", Difficulty="Hard", ImageLocation="swiftkey.png"},
            new Game{Name="game2", Difficulty="Easy",ImageLocation="ukimage.png"},
            new Game{Name="game3", Difficulty="medium",ImageLocation="ukimage.png"}

        };
        SelectOefeningen.ItemsSource = Games;
           
    }
}


public class Game
{
    public string Name{ get; set; }
    public string Difficulty { get; set; }

    public string ImageLocation { get; set; }


}


