﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            List<UserControl> properties = new List<UserControl>();
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
            _players.Add(new Player(SearchNextId(), name, balance, position, new List<UserControl>(), null));

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
        public static List<Player> ListAllPlayer()
        {
            return _players;
        }

        /// <summary>
        ///  Génère deux nombres aléatoire compris entre 1 et 6, simulant un lancé de dés.
        /// </summary>
        /// <returns> la valeur des deux dés sous forme de liste </returns>
        public static List<int> RollDice()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            List<int> dices = new List<int>();
            int dice1 = r.Next(1, 7);
            int dice2 = r.Next(1, 7);

            dices.Add(dice1);
            dices.Add(dice2);

            return dices;
        }

        /// <summary>
        /// Lance les dés et déplace le joueur de la valeur des dés.
        /// </summary>
        /// <param name="idPlayer"> Id du joueur qui va être déplacé</param>
        public static void MoovePlayer(int idPlayer)
        {
            Player p = SearchPlayer(idPlayer);
            Random r = new Random((int)DateTime.Now.Ticks);

            List<int> dices = RollDice();
            int dice1 = dices[0];
            int dice2 = dices[1];    
      
            if (dice1 == dice2)
            {
                p.NumberDoubleDice++; 
            }
            if (p.CanMoove)
            {
                p.Moove(dice1, dice2);
            }
            else
            {
                int positionPrison = 2;
                MoovePlayer(p.IdPlayer, positionPrison);
                Console.WriteLine("Le joueur est déplacé en prison");
            }
         }

        /// <summary>
        ///  Déplace le joueur à la case indiquée. 
        /// </summary>
        /// <param name="idPlayer"> Id du joueur qui doit être déplacé.</param>
        /// <param name="position"> Position de la case où le joueur doit être déplacé.</param>
        public static void MoovePlayer(int idPlayer, int position)
        {
            Player p = SearchPlayer(idPlayer);
            p.Position = position;
        }


        #endregion


    }
}
