using Monopoly.Model.Board;
using Monopoly.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    class CardManager
    {
        public static void DisplayCard(BaseCard baseCard)
        {
            Board board = Board.GetBoard;
            Grid.SetColumn(baseCard, 4);
            Grid.SetRow(baseCard, 4);
            Grid.SetRowSpan(baseCard, 2);
            Grid.SetColumnSpan(baseCard, 4);

            board.Children.Add(baseCard);

        }

        public static ChanceCard DrawChanceCard()
        {
           List<ChanceCard> deck =  Board.GetBoard.ChanceCardList;


            Random rnd = new Random();
            int random = rnd.Next(0, deck.Count());

            //Implementer la carte sortir de prison et le retrait de celle ci du packet quand draw + rajout quand utiliser

            return deck[random];
        }
    }
}
