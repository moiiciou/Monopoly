using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Monopoly.Model;

using Monopoly.Model.Board;
using System.Threading;
using server;
using Monopoly.Model.UI;
using Monopoly.Model.Case;
using server.Model;
using Newtonsoft.Json;

namespace Monopoly.Controller
{

    public static class PlayerManager
    {
        #region Attributs
        private static List<Player> _players = new List<Player>();
        public static volatile List<Grid> playerGrid = new List<Grid>();

        #endregion

  
        public static string CurrentPlayerName = "ERROR_NAME";
        public static int CurrentPlayerLastPosition = 0;



        public static void InitGrid(Player p)
        {
            playerGrid.Add(new BoardLayout());
            p.grid = 0;
        }

        /// <summary>
        /// Créé un nouveau joueur avec les paramètres nom, argent et position.
        /// </summary>
        /// <param name="name"> Nom du joueur </param>
        /// <param name="balance"> Argent du joueur </param>
        /// <param name="position"> Position sur le plateau du joueur </param>
        /// <return> Renvoie l'id du joueur créé. </return>
        public static bool CreatePlayer(Board board, string name, int balance, int position)
        {
            Player p = new Player( name, balance, position, new List<PropertyInfo>(), null, null);
            _players.Add(p);
            InitGrid(p);
            if (position >= 0)
            {
                DrawPlayer(board, p.playerInfo.Pseudo, 0);

            }



            return true;
        }

        public static PlayerInfo GetPlayerByPseuso(string pseudo)
        {
            foreach (PlayerInfo player in GameManager.MonopolyGameData.PlayerList)
            {
                if (player.Pseudo == pseudo)
                    return player;
            }

            return new PlayerInfo();
        }

        public static Player SearchPlayer(string pseudo)
        {
            Player player = null;

            for (int i = 0; i < _players.Count && player == null; i++)
            {
                if (_players[i].playerInfo.Pseudo == pseudo)
                {
                    player = _players[i];
                }
            }
            return player;
        }





        /// <summary>
        ///  Déplace le joueur à la case indiquée. 
        /// </summary>
        /// <param name="idPlayer"> Id du joueur qui doit être déplacé.</param>
        /// <param name="position"> Position de la case où le joueur doit être déplacé.</param>
        public static void MoovePlayer(Board b, string pseudo, int position)
        {
            Player p = SearchPlayer(pseudo);

            position = position % 40;
            DrawPlayer(b, pseudo, position);
            p.playerInfo.Position = position;

            if (BuyAndSellManager.CheckIfBuyable(b.CasesList[position]) & CurrentPlayerLastPosition != position & b.CasesList[position].GetType() == typeof(PropertyCase))
            {
                PropertyCase propertyCase = (PropertyCase)b.CasesList[position];
                BuyDialog buyDialog = new BuyDialog(propertyCase.Card.CardInformation.Location, propertyCase.CaseInformation.TxtPrice);
                b.Children.Remove(buyDialog);

                if (CurrentPlayerName.Trim('0') != propertyCase.CaseInformation.Owner & pseudo == CurrentPlayerName.Trim('0'))
                {
                    Grid.SetColumn(buyDialog, 4);
                    Grid.SetRow(buyDialog, 4);
                    Grid.SetRowSpan(buyDialog, 8);
                    Grid.SetColumnSpan(buyDialog, 8);

                    b.Children.Add(buyDialog);
                }

            }
            
            if(b.CasesList[position].GetType() == typeof(ChanceCase))
            {
                Packet packet = new Packet();
                packet.Type = "drawChance";
                packet.Content = "";
                packet.ChatMessage = "pioche une carte";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
            }

            if (b.CasesList[position].GetType() == typeof(CommunityCase))
            {
                Packet packet = new Packet();
                packet.Type = "drawCommunity";
                packet.Content = "";
                packet.ChatMessage = "pioche une carte";
                string packetToSend = JsonConvert.SerializeObject(packet, Formatting.Indented);
                Connection.SendMsg(packetToSend);
            }
        }



        public static void DrawPlayer(Board b, string pseudo, int pos)
        {

            Player p = SearchPlayer(pseudo);
            b.Children.Remove(p);
            pos = pos % 40;
            int x = b.CasesList[pos].Position[1];
            int y = b.CasesList[pos].Position[0];
            Grid.SetColumn(p, x);
            Grid.SetRow(p, y);
            b.Children.Add(p);


        }


    }
}
