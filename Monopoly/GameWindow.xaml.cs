using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;
using Monopoly.Model.UI;
using Monopoly.Controller;
using System.Threading.Tasks;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        List<int> players = new List<int>();
        Board board = Board.GetBoard;
        object lockBoard = new object();
        List<Task> tasks = new List<Task>();

        public GameWindow()
        {
            InitializeComponent();
            Board board = Board.GetBoard;
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);

            players.Add(PlayerManager.CreatePlayer(board, "test", 10000, 0));
            List<int> dices = PlayerManager.RollDice();
            
            PlayerManager.MoovePlayer(board, players[0]);
      

            PlayerInterface playerHud = new PlayerInterface();
            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {






        }


    }
}



