using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Monopoly.Model;

using Monopoly.Model.Board;
using System.Threading;
using Monopoly.Model.Card;

namespace Monopoly.Controller
{

    public static class PlayerManager
    {
        #region Attributs
        private static List<Player> _players = new List<Player>();
        private static int _nextId = 0;
        private static Player _bank;
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
        public static int CreatePlayer(Board board, string name, int balance, int position)
        {
            Player p = new Player(SearchNextId(), name, balance, position, new List<BaseCard>(), null);
            _players.Add(p);
            InitGrid(p);
            if (position >= 0)
            {
                DrawPlayer(board, p.IdPlayer);

            }



            return _players.Last().IdPlayer;
        }

        /// <summary>
        /// Calcule et fourni le nouvel ID d'un joueur.
        /// </summary>
        /// <returns> Renvoie le nouvel ID du joueur </returns>
        public static int SearchNextId()
        {
            return _nextId++;
        }



        /// <summary>
        ///  Cherche un joueur avec son id dans la liste des joueurs créés.
        /// </summary>
        /// <param name="idPlayer"> Id du joueur que l'on recherche </param>
        /// <returns> Le Player dont l'id correspond </returns>
        public static Player SearchPlayer(int idPlayer)
        {
            Player player = null;

            for (int i = 0; i < _players.Count && player == null; i++)
            {
                if (_players[i].IdPlayer == idPlayer)
                {
                    player = _players[i];
                }
            }
            return player;
        }
        public static Player SearchPlayer(string pseudo)
        {
            Player player = null;

            for (int i = 0; i < _players.Count && player == null; i++)
            {
                if (_players[i].NamePlayer == pseudo)
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
            if(position != position % 40)
            {
                p.AddAmount(500);
            }
            position = position % 40;
            DrawPlayer(b, pseudo, position);

        }

        public static void DrawPlayer(Board b, int idPlayer)
        {
            Console.WriteLine(SearchPlayer(idPlayer).Position);
            Player p = SearchPlayer(idPlayer);
            b.Children.Remove(p);
            Console.WriteLine(b.CasesList[p.Position]);
            
            int x = b.CasesList[p.Position].Position[1];
            int y = b.CasesList[p.Position].Position[0];
            Grid.SetColumn(p, x);
            Grid.SetRow(p, y);
            b.Children.Add(p);

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
