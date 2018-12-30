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
        private bool _gameOver = false;

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


            PlayerInterface playerHud = new PlayerInterface(PlayerManager.SearchPlayer(0));
            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);

            List<int> dices = PlayerManager.RollDice();

            PlayTurn(players);

        }

        private static  void PlayTurn(List<int> players)
        {
            bool _gameOver = false;
  
                foreach (int player in players)
                {
                    PlayerManager.MoovePlayer(Board.GetBoard, player);
                    var testdial = new Model.UI.DialogueBox("C'est ton tour connard !");
                    testdial.SetValue(Grid.ColumnSpanProperty, 4);
                    Grid.SetRow(testdial, 3);
                    Grid.SetColumn(testdial, 3);
                    Board.GetBoard.Children.Add(testdial);
                }

     

        }


    }
}
