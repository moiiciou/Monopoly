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

namespace Monopoly.Controller
{

    public static class PlayerManager
    {
        #region Attributs
        private static List<Player> _players = new List<Player>();
        public static volatile List<Grid> playerGrid = new List<Grid>();

        #endregion

  
        public static string CurrentPlayerName = "ERROR_NAME";



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
            Player p = new Player( name, balance, position, new List<CaseInfo>(), null, null);
            _players.Add(p);
            InitGrid(p);
            if (position >= 0)
            {
                DrawPlayer(board, p.playerInfo.Pseudo, 0);

            }



            return true;
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
            if(BuyAndSellManager.CheckIfBuyable(b.CasesList[position]))
            {
                BuyAndSellManager.BuyProperty(b.CasesList[position], p);
            }
            BuyAndSellManager.PayRent(b.CasesList[position], p);
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
