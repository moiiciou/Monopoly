using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monopoly.Model;

namespace Monopoly.Controller
{
    public static class PlayerManager
    {
        #region Attributs
        private static List<Player> _players = new List<Player>();
        private static int _nextId = 0;
        private static Player _bank;
            
        #endregion

        #region Méthodes de gestion des joueurs
        /// <summary>
        /// Initialise la banque. C'est une entitée Player, elle à juste de l'argent "infini" et un id 0.
        /// TODO : Une fois les cartes initialisées les mettre dans la banque. SAUF pour les cartes Chance et Caisse de communauté
        /// </summary>
        /// <returns> L'id de la banque</returns>
        public static int CreateBank()
        {
            int bankId = SearchNextId();
            List<Card> properties = new List<Card>();
            _bank = new Player(bankId, "Bank", int.MaxValue, 0, properties, null);
            _players.Add(_bank);

            return bankId;

        }
        /// <summary>
        /// Créé un nouveau joueur avec les paramètres nom, argent et position.
        /// </summary>
        /// <param name="name"> Nom du joueur </param>
        /// <param name="balance"> Argent du joueur </param>
        /// <param name="position"> Position sur le plateau du joueur </param>
        /// <return> Renvoie l'id du joueur créé. </return>
        public static int CreatePlayer(string name, int balance,int position)
        {
            _players.Add(new Player(SearchNextId(), name, balance, position, new List<Card>(), null));

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
        ///  Effectue le paiement entre deux joueurs (le joueur Payer paie le joueur Reciever. 
        /// </summary>
        /// <param name="idReciever">Id du joueur Reciever</param>
        /// <param name="idPayer">Id du joueur Payer</param>
        /// <param name="amount">Montant de la transaction entre les deux joueurs</param>
        public static void Pay(int idReciever, int idPayer, int amount)
        {
            
            Player reciever = SearchPlayer(idReciever);
            Player payer = SearchPlayer(idPayer);

            if (idReciever == _bank.IdPlayer)
            {
                payer.SoustractAmount(amount);
            }
            else if (idPayer == _bank.IdPlayer)
            {
                reciever.AddAmount(amount);
            }
            else
            {
                reciever.AddAmount(amount);
                payer.SoustractAmount(amount);
            }
         }

        /// <summary>
        ///  Cherche un joueur avec son id dans la liste des joueurs créés.
        /// </summary>
        /// <param name="idPlayer"> Id du joueur que l'on recherche </param>
        /// <returns> Le Player dont l'id correspond </returns>
        public static Player SearchPlayer(int idPlayer)
        {
            Player player=null;
           
            for(int i=0; i<_players.Count && player==null; i++)
            {
                if(_players[i].IdPlayer== idPlayer)
                {
                    player = _players[i];
                }
            }
            return player;
        }

        /// <summary>
        ///  Renvoie la liste de tous les Players existant.
        /// </summary>
        /// <returns> La liste des Players existants</returns>
        public static List<Player> listAllPlayer()
        {
            return _players;
        }

        
        #endregion


    }
}
