using Monopoly.Model.Board;
using Monopoly.Model.UI;
using server;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    class GameManager
    {
        public static Dictionary<string, FrameworkElement> controls = new Dictionary<string, FrameworkElement>();
        public static Dictionary<string, PlayerInfo> playersList = new Dictionary<string, PlayerInfo>(); // string = PseudoPlayer

        public GameManager(Grid root)
        {

            object lockBoard = new object();
            List<Task> tasks = new List<Task>();

            Board board = Board.GetBoard;
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);

            PlayerInterface playerHud = new PlayerInterface(PlayerManager.CurrentPlayerName, server.GameServer.initBalance);
            controls.Add("playerHud", playerHud);
            controls.Add("board", board);
            controls.Add("grid", root);
            controls.Add("playerPanel", playerHud.PlayerPanel);

            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);




        }


    }
}
