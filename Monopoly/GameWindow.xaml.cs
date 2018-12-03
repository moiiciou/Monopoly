using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Monopoly.Properties;
using Monopoly.Model;
using System.Windows.Controls;
using Monopoly.Model.Board;
using Monopoly.Controller;

namespace Monopoly
{
    /// <summary>
    /// Logique d'interaction pour GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        List<int> players = new List<int>();
        Board board = new Board();

        public GameWindow()
        {
            InitializeComponent();
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);

            players.Add(PlayerManager.CreatePlayer(board, "test", 1500, 0));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player p = PlayerManager.SearchPlayer(players[0]);
            PlayerManager.MoovePlayer(p.IdPlayer,p.Position+1);
            //PlayerManager.MoovePlayer(players[0], 1);
            PlayerManager.DrawPlayer(board, players[0]);
            
        }
    }
}

  

