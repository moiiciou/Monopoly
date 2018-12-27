using Monopoly.Model.Board;
using Monopoly.Model.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    class GameManager
    {
        public GameManager(Grid root)
        {

            List<int> players = new List<int>();
            object lockBoard = new object();
            List<Task> tasks = new List<Task>();

            Board board = Board.GetBoard;
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);
            players.Add(PlayerManager.CreatePlayer(board, "test", 10000, 0));
           

            PlayerInterface playerHud = new PlayerInterface(PlayerManager.SearchPlayer(0).NamePlayer, PlayerManager.SearchPlayer(0).Balance);
            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);

            List<int> dices = PlayerManager.RollDice();


        }
    }
}
