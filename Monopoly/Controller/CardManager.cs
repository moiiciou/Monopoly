using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Monopoly.Controller
{
    class CardManager
    {

        private List<UserControl> ChanceDeck;
        private List<UserControl> CommunityDeck;


        /// <summary>
        /// Return the list of card in the game
        /// cardType : "all" "property" "community" "chance" 
        /// </summary>
        /// <returns></returns>
        public List<UserControl> GetCardList(string cardType = "all")
        {

        }

        /// <summary>
        /// return the deck list of the game
        /// </summary>
        /// <param name="deckType">"all" "chance" "community"</param>
        /// <returns></returns>
        public List<UserControl> GetDeckList(string deckType = "all")
        {

        }

        /// <summary>
        /// DrawCard from specified deck
        /// </summary>
        /// <returns></returns>
        public UserControl DrawCard(List<UserControl> deck)
        {

        }


    }
}
