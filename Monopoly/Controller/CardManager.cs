using Monopoly.Model.Board;
using Monopoly.Model.Card;
using server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    public static class CardManager
    {
        public static void DisplayCard(CardInfo cardInfo)
        {
            ChanceCard chanceCard = new ChanceCard(cardInfo.Label, cardInfo.Text, cardInfo.Effect);
            Grid.SetColumn(chanceCard, 4);
            Grid.SetRow(chanceCard, 4);
            Grid.SetRowSpan(chanceCard, 2);
            Grid.SetColumnSpan(chanceCard, 3);
            Board.GetBoard.Children.Add(chanceCard);

        }

    }
}
