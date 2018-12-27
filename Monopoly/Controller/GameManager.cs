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
            Board board = Board.GetBoard;
            Grid.SetRow(board, 0);
            Grid.SetColumn(board, 0);
            root.Children.Add(board);

            PlayerInterface playerHud = new PlayerInterface();
            Grid.SetRow(playerHud, 0);
            Grid.SetColumn(playerHud, 1);
            root.Children.Add(playerHud);

        }
    }
}
