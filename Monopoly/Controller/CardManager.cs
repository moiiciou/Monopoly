using Monopoly.Model.Card;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static Monopoly.Model.Board.Board;

namespace Monopoly.Controller
{
    class CardManager
    {

        private List<ChanceCard> ChanceDeck;
        private List<ChanceCard> CommunityDeck;
        private List<PropertyCard> PropertyDeck;



        /// <summary>
        /// DrawCard from specified deck
        /// </summary>
        /// <returns></returns>
        public object DrawCard(List<object> deck)
        {
            return null;
        }

        /// <summary>
        /// replace card in specified deck
        /// </summary>
        /// <returns></returns>
        public void ReplaceToDeck(object card, List<object> deck)
        {

        }


    }
}
